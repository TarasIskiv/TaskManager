using TaskManager.Cache.Abstraction;
using TaskManager.Core.Payloads;
using TaskManager.Team.Logic.Abstraction;
using TaskManager.Team.Repository.Abstraction;

namespace TaskManager.Team.Logic.Implementation;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly ICacheService _cacheService;

    public TeamService(ITeamRepository teamRepository, ICacheService cacheService)
    {
        _teamRepository = teamRepository;
        _cacheService = cacheService;
    }
    public async Task CreateUser(UpsertUserPayload payload)
    {
        var userId = await _teamRepository.CreateUser(payload);
        if(userId == default) return;
    }

    public async Task RemoveUser(int userId)
    {
        await _teamRepository.RemoveUser(userId);
    }
}