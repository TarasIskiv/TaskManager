using TaskManager.Core.Payloads;
using TaskManager.Core.Responses;

namespace TaskManager.Team.Logic.Abstraction;

public interface ITeamService
{
    Task CreateUser(CreateUserPayload payload);
    Task RemoveUser(int userId);
    Task UpdateUser(UpdateUserPayload payload);
    Task<UserResponse> GetSingleUser(int userId);
    Task<List<UserResponse>> GetUsers();
}