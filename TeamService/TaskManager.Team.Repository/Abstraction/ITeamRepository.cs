using TaskManager.Core.Payloads;

namespace TaskManager.Team.Repository.Abstraction;

public interface ITeamRepository
{
    Task<int> CreateUser(CreateUserPayload payload);
    Task RemoveUser(int userId);
    Task UpdateUser(UpdateUserPayload payload);
}