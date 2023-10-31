using TaskManager.Core.Payloads;

namespace TaskManager.Task.Repository.Abstraction;

public interface IUserRepository
{
    Task<int> CreateUser(UserContactInfo payload);
    System.Threading.Tasks.Task RemoveUser(int userId);
    Task<UserContactInfo> GetUser(int userId);
    Task<List<UserContactInfo>> GetUsers();
}