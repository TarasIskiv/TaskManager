namespace TaskManager.Core.Payloads;

public class UserContactInfo
{
    public string Email { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Surname { get; set; } = default!;
}