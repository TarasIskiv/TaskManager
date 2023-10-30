namespace TaskManager.Core.Responses;

public record struct UserResponse
{
    public int UserId { get; init; }
    
    public string Email { get; init; } 
    
    public string Name { get; init; } 
    
    public string Surname { get; init; } 
    
    public string? Role { get; init; } 
    
    public DateTime? DateOfBirth { get; init; } 
    
    public float? Salary { get; init; }

    public string? Nationality { get; init; } 
    
    public DateTime? WorkSince { get; init; } 
}