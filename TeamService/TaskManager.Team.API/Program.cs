using Microsoft.Extensions.Options;
using TaskManager.Cache.Abstraction;
using TaskManager.Cache.Implementation;
using TaskManager.Core.QueueConfig;
using TaskManager.Database;
using TaskManager.MessageBroker.Abstraction;
using TaskManager.MessageBroker.Implementation;
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
builder.Services.Configure<QueueBaseConfig>(builder.Configuration.GetSection("MessageBroker"));

builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<IQueueService, QueueService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
