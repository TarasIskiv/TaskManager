using TaskManager.Core.NotificationConfig;
using TaskManager.Core.QueueConfig;
using TaskManager.MessageBroker.Abstraction;
using TaskManager.MessageBroker.Implementation;
using TaskManager.Notification.Logic.Abstraction;
using TaskManager.Notification.Logic.Implementation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<MessageListener>();

builder.Services.Configure<QueueBaseConfig>(builder.Configuration.GetSection("MessageBroker"));
builder.Services.Configure<NotificationConfig>(builder.Configuration.GetSection("NotificationConfig"));

builder.Services.AddScoped<INotificationSenderService, NotificationSenderService>();
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
