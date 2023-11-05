using TaskManager.Core.Payloads;
using TaskManager.Core.Responses;

namespace TaskManager.Task.Logic.Abstraction;

public interface IUserService
{
    System.Threading.Tasks.Task CreateUser(int userId, UserContactInfo payload);
    System.Threading.Tasks.Task RemoveUser(int userId);
    Task<UserContactInfoResponse> GetUser(int userId);
    Task<List<UserContactInfoResponse>> GetUsers();
}