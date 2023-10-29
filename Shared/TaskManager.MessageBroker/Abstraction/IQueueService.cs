namespace TaskManager.MessageBroker.Abstraction;

public interface IQueueService
{
    Task PushMessage<T>(T message);
    T ReceiveMessage<T>();
}