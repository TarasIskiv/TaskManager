namespace TaskManager.Core.QueueConfig;

public class QueueMessageConfig
{
    public string QueueName { get; set; } = default!;
    public string ExchangeName { get; set; } = default!;
    public string RoutingKeyName { get; set; } = default!;

}