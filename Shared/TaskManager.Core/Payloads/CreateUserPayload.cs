using System.ComponentModel.DataAnnotations;

namespace TaskManager.Core.Payloads;

public class CreateUserPayload
{
    [EmailAddress, Required, MaxLength(50)]
    public string Email { get; set; } = default!;
    
    [Required, MaxLength(30)]
    public string Name { get; set; } = default!;
    
    [Required, MaxLength(30)]
    public string Surname { get; set; } = default!;
    
    [Required, MaxLength(40)]
    public string Role { get; set; } = default!;
    
    public DateTime DateOfBirth { get; set; } = default!;
    
    public float Salary { get; set; }

    public string Nationality { get; set; } = default!;
    
    public DateTime WorkSince { get; set; } = default!;

}