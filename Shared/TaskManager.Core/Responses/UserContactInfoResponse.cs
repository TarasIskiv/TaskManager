namespace TaskManager.Core.Responses;

public class UserContactInfoResponse
{
    public int UserId { get; set; }
    public string Email { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Surname { get; set; } = default!;
}