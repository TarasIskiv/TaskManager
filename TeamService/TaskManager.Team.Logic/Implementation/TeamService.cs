using TaskManager.Cache.Abstraction;
using TaskManager.Core.Payloads;
using TaskManager.Core.Responses;
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
    public async Task CreateUser(CreateUserPayload payload)
    {
        var userId = await _teamRepository.CreateUser(payload);
        if(userId == default) return;
    }

    public async Task RemoveUser(int userId)
    {
        await _teamRepository.RemoveUser(userId);
    }

    public async Task UpdateUser(UpdateUserPayload payload)
    {
        await _teamRepository.UpdateUser(payload);
    }

    public async Task<UserResponse> GetSingleUser(int userId)
    {
        return await _teamRepository.GetSingleUser(userId);
    }

    public async Task<List<UserResponse>> GetUsers()
    {
        return await _teamRepository.GetUsers();
    }
}