using TaskManager.Core.QueueConfig;

namespace TaskManager.MessageBroker.Abstraction;

public interface IQueueService
{
    Task PushMessage<T>(T message, string queueName);
    T ReceiveMessage<T>(string queueName);
    string GetQueueName(QueueConnection connection);
}