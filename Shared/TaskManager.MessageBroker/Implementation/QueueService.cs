using System.Data;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TaskManager.MessageBroker.Abstraction;

namespace TaskManager.MessageBroker.Implementation;

public class QueueService : IQueueService
{
    private ConnectionFactory _factory;
    private IConnection _connection;
    public QueueService()
    {
        DefineFactory();
    }

    private void DefineFactory()
    {
        _factory = new();
        _factory.Uri = new("");
        _factory.ClientProvidedName = "";
    }

    private IChannel SetupChannel()
    {
        _connection = _factory.CreateConnection();
        IChannel channel = _connection.CreateChannel();
        channel.ExchangeDeclare("", ExchangeType.Direct);
        channel.QueueDeclare("", false, false, false, null);
        channel.QueueBind("", "", "", null);
        channel.BasicQos(0, 1, false);
        return channel;
    }
    
    public async Task PushMessage<T>(T message)
    {
        var channel = SetupChannel();
        var messageBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        await channel.BasicPublishAsync("", "", messageBody);
        channel.Close();
        _connection.Close();
    }

    public T ReceiveMessage<T>()
    {
        var channel = SetupChannel();
        var consumer = new EventingBasicConsumer(channel);
        string message = string.Empty;
        consumer.Received += (model, mes) =>
        {
            var body = mes.Body.ToArray();
            message = Encoding.UTF8.GetString(body);
        };
        channel.BasicConsume("", true, consumer);
        channel.Close();
        _connection.Close();
        var deserializedMessage = JsonSerializer.Deserialize<T>(message);
        return deserializedMessage ?? default(T)!;
    }
}