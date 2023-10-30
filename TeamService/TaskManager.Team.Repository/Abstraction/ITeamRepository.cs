using TaskManager.Core.Payloads;
using TaskManager.Core.Responses;

namespace TaskManager.Team.Repository.Abstraction;

public interface ITeamRepository
{
    Task<int> CreateUser(CreateUserPayload payload);
    Task RemoveUser(int userId);
    Task UpdateUser(UpdateUserPayload payload);
    Task<UserResponse> GetSingleUser(int userId);
    Task<List<UserResponse>> GetUsers();
}