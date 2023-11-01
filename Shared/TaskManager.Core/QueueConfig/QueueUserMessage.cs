using TaskManager.Core.Enums;
using TaskManager.Core.Payloads;

namespace TaskManager.Core.QueueConfig;

public class QueueUserMessage
{
    public int UserId { get; set; }
    public UserActionType ActionType { get; set; }
    public UserContactInfo? User { get; set; } = default!;
}