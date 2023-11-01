using TaskManager.Core.Payloads;
using TaskManager.Core.Responses;

namespace TaskManager.Core.CustomMapper;

public static class Mapper
{
    public static UserContactInfo MapToUserContactInfo(this UserResponse user)
    {
        return new UserContactInfo()
        {
            Email = user.Email,
            Name = user.Name,
            Surname = user.Surname
        };
    }
}