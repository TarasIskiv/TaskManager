using TaskManager.Core.QueueConfig;

namespace TaskManager.MessageBroker.Abstraction;

public interface IQueueService
{
    Task PushMessage<T>(T message,QueueMessageConfig config);
    T ReceiveMessage<T>(QueueMessageConfig config);
    QueueMessageConfig GetQueueConfiguration(QueueConnection connection);
}