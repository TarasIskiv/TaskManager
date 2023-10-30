using System.ComponentModel.DataAnnotations;

namespace TaskManager.Core.Payloads;

public class UpdateUserPayload
{
    [Required]
    public int UserId { get; set; }
    [Required, MaxLength(40)]
    public string Role { get; set; } = default!;
    
    public DateTime DateOfBirth { get; set; } = default!;
    
    public float Salary { get; set; }

    public string Nationality { get; set; } = default!;
    
    public DateTime WorkSince { get; set; } = default!;
}