using System.Data;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TaskManager.Core.QueueConfig;
using TaskManager.MessageBroker.Abstraction;

namespace TaskManager.MessageBroker.Implementation;

public class QueueService : IQueueService
{
    private ConnectionFactory _factory;
    private IConnection _connection;
    private readonly QueueConfig _config;
    public QueueService(IOptions<QueueConfig> config)
    {
        _config = config.Value;
        DefineFactory();
    }

    private void DefineFactory()
    {
        _factory = new();
        _factory.Uri = new(_config.Uri);
        _factory.ClientProvidedName = _config.ClientName;
    }

    private IChannel SetupChannel(string queueName)
    {
        _connection = _factory.CreateConnection();
        IChannel channel = _connection.CreateChannel();
        channel.ExchangeDeclare(_config.ExchangeName, ExchangeType.Direct);
        channel.QueueDeclare(queueName, false, false, false, null);
        channel.QueueBind(queueName, _config.ExchangeName, _config.RoutingKey, null);
        channel.BasicQos(0, 1, false);
        return channel;
    }
    
    public async Task PushMessage<T>(T message, string queueName)
    {
        var channel = SetupChannel(queueName);
        var messageBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        await channel.BasicPublishAsync(_config.ExchangeName, _config.RoutingKey, messageBody);
        channel.Close();
        _connection.Close();
    }

    public T ReceiveMessage<T>(string queueName)
    {
        var channel = SetupChannel(queueName);
        var consumer = new EventingBasicConsumer(channel);
        string message = string.Empty;
        consumer.Received += (model, mes) =>
        {
            var body = mes.Body.ToArray();
            message = Encoding.UTF8.GetString(body);
        };
        channel.BasicConsume(queueName, true, consumer);
        channel.Close();
        _connection.Close();
        var deserializedMessage = JsonSerializer.Deserialize<T>(message);
        return deserializedMessage ?? default(T)!;
    }

    public string GetQueueName(QueueConnection connection)
    {
        return connection switch
        {
            QueueConnection.TaskNotificationConnection => "TaskNotificationQueue",
            QueueConnection.TaskTeamConnection => "TaskTeamQueue"
        };
    }
}