using System.ComponentModel.DataAnnotations;
using TaskManager.Core.Enums;

namespace TaskManager.Core.Payloads;

public class CreateTaskPayload
{
    [Required, MaxLength(70)]
    public string Title { get; set; } = default!;
    public int AssignedTo { get; set; } = default!;
    public Priority Priority { get; set; } = Priority.Low;
    public Status Status { get; set; } = Status.New;
}