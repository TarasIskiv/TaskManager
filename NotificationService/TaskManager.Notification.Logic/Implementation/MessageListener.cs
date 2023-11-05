using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskManager.Core.Enums;
using TaskManager.Core.QueueConfig;
using TaskManager.MessageBroker.Abstraction;
using TaskManager.Notification.Logic.Abstraction;

namespace TaskManager.Notification.Logic.Implementation;

public class MessageListener : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public MessageListener(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var _queueService = scope.ServiceProvider.GetRequiredService<IQueueService>();
        var _notificationSenderService = scope.ServiceProvider.GetRequiredService<INotificationSenderService>();
        var queueConfig = _queueService.GetQueueConfiguration(QueueConnection.TaskNotificationConnection);
        while (!stoppingToken.IsCancellationRequested)
        {
            var queueMessage = _queueService.ReceiveMessage<QueueNotificationMessage?>(queueConfig);
            if (queueMessage != default(QueueNotificationMessage))
            {
                await _notificationSenderService.SendNotification(queueMessage);
            }
            await System.Threading.Tasks.Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}