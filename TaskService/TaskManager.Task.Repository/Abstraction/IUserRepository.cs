using TaskManager.Core.Payloads;
using TaskManager.Core.Responses;

namespace TaskManager.Task.Repository.Abstraction;

public interface IUserRepository
{
    System.Threading.Tasks.Task CreateUser(UserContactInfo payload);
    System.Threading.Tasks.Task RemoveUser(int userId);
    Task<UserContactInfoResponse> GetUser(int userId);
    Task<List<UserContactInfoResponse>> GetUsers();
}