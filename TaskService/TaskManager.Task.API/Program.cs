using TaskManager.Cache.Abstraction;
using TaskManager.Cache.Implementation;
using TaskManager.Core.QueueConfig;
using TaskManager.Database;
using TaskManager.MessageBroker.Abstraction;
using TaskManager.MessageBroker.Implementation;
using TaskManager.Task.Logic.Abstraction;
using TaskManager.Task.Logic.Implementation;
using TaskManager.Task.Repository.Abstraction;
using TaskManager.Task.Repository.Implementation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.Configuration = builder.Configuration.GetConnectionString("Redis");
    opt.InstanceName = "TaskManager/Team/";
});

builder.Services.AddHostedService<MessageListener>();
builder.Services.AddSingleton<DapperContext>();
builder.Services.Configure<QueueBaseConfig>(builder.Configuration.GetSection("MessageBroker"));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IQueueService, QueueService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
