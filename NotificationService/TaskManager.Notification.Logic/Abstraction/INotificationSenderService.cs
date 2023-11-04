using TaskManager.Core.QueueConfig;

namespace TaskManager.Notification.Logic.Abstraction;

public interface INotificationSenderService
{
    Task SendNotification(QueueNotificationMessage message);
}