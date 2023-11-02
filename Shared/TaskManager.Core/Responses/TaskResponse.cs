using TaskManager.Core.Enums;

namespace TaskManager.Core.Responses;

public record struct TaskResponse
{
    public int TaskId { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public string AcceptanceCriteria { get; init; }
    public Status StatusInfo { get; init; }
    public int UserId { get; init; }
    public string AssignedTo { get; init; }
    public int StoryPoints { get; init; }
    public Priority PriorityInfo { get; init; }
    public DateTime CreationDate { get; init; }
}