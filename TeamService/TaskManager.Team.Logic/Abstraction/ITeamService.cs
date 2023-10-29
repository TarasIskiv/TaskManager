using TaskManager.Core.Payloads;

namespace TaskManager.Team.Logic.Abstraction;

public interface ITeamService
{
    Task CreateUser(UpsertUserPayload payload);
}