using TaskManager.Core.QueueConfig;
using TaskManager.Database;
using TaskManager.Team.Logic.Abstraction;
using TaskManager.Team.Logic.Implementation;
using TaskManager.Team.Repository.Abstraction;
using TaskManager.Team.Repository.Implementation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.Configuration = builder.Configuration.GetConnectionString("Redis");
    opt.InstanceName = "TaskManager/Team/";
});

builder.Services.AddSingleton<DapperContext>();
builder.Services.Configure<QueueConfig>(builder.Configuration.GetSection("MessageBroker"));

builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
