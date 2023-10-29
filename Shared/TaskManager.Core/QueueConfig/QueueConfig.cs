namespace TaskManager.Core.QueueConfig;

public class QueueConfig
{
    public string Uri { get; set; } = default!;
    public string ClientName { get; set; } = default!;
    public string ExchangeName { get; set; } = default!;
    public string RoutingKey { get; set; } = default!;
}