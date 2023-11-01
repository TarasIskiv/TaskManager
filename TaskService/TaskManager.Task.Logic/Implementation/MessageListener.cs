using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskManager.Core.Enums;
using TaskManager.Core.QueueConfig;
using TaskManager.MessageBroker.Abstraction;
using TaskManager.Task.Logic.Abstraction;

namespace TaskManager.Task.Logic.Implementation;

public class MessageListener : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public MessageListener(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    protected override async System.Threading.Tasks.Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var _queueService = scope.ServiceProvider.GetRequiredService<IQueueService>();
        var _userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        var queueName = _queueService.GetQueueName(QueueConnection.TaskTeamConnection);
        while (!stoppingToken.IsCancellationRequested)
        {
            var queueMessage = _queueService.ReceiveMessage<QueueUserMessage>(queueName);
            if (queueMessage.ActionType == UserActionType.Create)
            {
                await _userService.CreateUser(queueMessage.UserId, queueMessage!.User!);
            }
            else
            {
                await _userService.RemoveUser(queueMessage.UserId);
            }
            await System.Threading.Tasks.Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}