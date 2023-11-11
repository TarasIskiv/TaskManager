using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

var path = builder.Environment.IsDevelopment() ? "ocelot.Development.json" : "ocelot.Production.json";
builder.Configuration.AddJsonFile(path, false, true);
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapControllers();
await app.UseOcelot();

app.Run();