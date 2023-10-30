using TaskManager.Core.Payloads;

namespace TaskManager.Team.Repository.Abstraction;

public interface ITeamRepository
{
    Task<int> CreateUser(UpsertUserPayload payload);
    Task RemoveUser(int userId);

}