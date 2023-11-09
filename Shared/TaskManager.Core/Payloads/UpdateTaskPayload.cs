using System.ComponentModel.DataAnnotations;
using TaskManager.Core.Enums;

namespace TaskManager.Core.Payloads;

public class UpdateTaskPayload
{
    public int TaskId { get; set; }
    [Required, MaxLength(70)]
    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;
    public string AcceptanceCriteria { get; set; } = default!;
    public int StoryPoints { get; set; }
    public int AssignedTo { get; set; } = default!;
    public Priority Priority { get; set; } = Priority.Low;
    public Status Status { get; set; } = Status.New;
}