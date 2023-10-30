using TaskManager.Core.Payloads;

namespace TaskManager.Team.Logic.Abstraction;

public interface ITeamService
{
    Task CreateUser(CreateUserPayload payload);
    Task RemoveUser(int userId);
    Task UpdateUser(UpdateUserPayload payload);
}