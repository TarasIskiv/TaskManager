using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using TaskManager.Core.QueueConfig;
using TaskManager.MessageBroker.Abstraction;

namespace TaskManager.MessageBroker.Implementation;

public class QueueService : IQueueService
{
    private ConnectionFactory _factory = default!;
    private IConnection _connection = default!;
    private readonly QueueBaseConfig _baseConfig;
    public QueueService(IOptions<QueueBaseConfig> config)
    {
        _baseConfig = config.Value;
        DefineFactory();
    }

    private void DefineFactory()
    {
        _factory = new();
        _factory.Uri = new(_baseConfig.Uri);
        _factory.ClientProvidedName = _baseConfig.ClientName;
    }

    private IChannel SetupChannel(QueueMessageConfig config)
    {
        _connection = _factory.CreateConnection();
        IChannel channel = _connection.CreateChannel();
        channel.ExchangeDeclare(config.ExchangeName, ExchangeType.Direct);
        channel.QueueDeclare(config.QueueName, false, false, false, null);
        channel.QueueBind(config.QueueName, config.ExchangeName, config.RoutingKeyName, null);
        channel.BasicQos(0, 1, false);
        return channel;
    }
    
    public async Task PushMessage<T>(T message,QueueMessageConfig config)
    {
        var channel = SetupChannel(config);
        var messageBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        await channel.BasicPublishAsync(config.ExchangeName, config.RoutingKeyName, messageBody);
        channel.Close();
        _connection.Close();
    }

    public T ReceiveMessage<T>(QueueMessageConfig config)
    {
        var channel = SetupChannel(config);
        var result = channel.BasicGet(config.QueueName, true);
        if (result is null) return default(T)!;
        var body = result.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        channel.Close();
        _connection.Close();
        if (string.IsNullOrEmpty(message)) return default(T)!;
        var deserializedMessage = JsonSerializer.Deserialize<T>(message);
        return deserializedMessage ?? default(T)!;
    }

    public QueueMessageConfig GetQueueConfiguration(QueueConnection connection)
    {
        return connection switch
        {
            QueueConnection.TaskNotificationConnection => new QueueMessageConfig()
            {
                QueueName = "TaskNotificationQueue",
                ExchangeName = "TaskManagerNotificationExchange",
                RoutingKeyName = "TaskManagerNotificationRoutingKey"
            } ,
            QueueConnection.TaskTeamConnection =>new QueueMessageConfig()
            {
                QueueName = "TaskTeamQueue",
                ExchangeName = "TaskManagerExchange",
                RoutingKeyName = "TaskManagerRoutingKey"
            },
            _ => new QueueMessageConfig()
            {
                QueueName = "TaskNotificationQueue",
                ExchangeName = "TaskManagerNotificationExchange",
                RoutingKeyName = "TaskManagerNotificationRoutingKey"
            } 
        };
    }
}