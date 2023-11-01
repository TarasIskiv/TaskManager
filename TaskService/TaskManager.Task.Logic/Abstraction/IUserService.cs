using TaskManager.Core.Payloads;

namespace TaskManager.Task.Logic.Abstraction;

public interface IUserService
{
    System.Threading.Tasks.Task CreateUser(int userId, UserContactInfo payload);
    System.Threading.Tasks.Task RemoveUser(int userId);
    Task<UserContactInfo> GetUser(int userId);
    Task<List<UserContactInfo>> GetUsers();
}