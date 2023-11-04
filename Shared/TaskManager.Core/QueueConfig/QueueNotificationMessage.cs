namespace TaskManager.Core.QueueConfig;

public class QueueNotificationMessage
{
    public string ReceiverEmail { get; set; } = default!;
    public string ReceiverFullName { get; set; } = default!;
    public string Subject { get; set; } = default!;
    public string Context { get; set; } = default!;
}