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
        var user = await _teamRepository.GetSingleUser(userId);
        var key = _cacheService.GetUserKey(userId);
        await _cacheService.SetData(key, user);
    }

    public async Task RemoveUser(int userId)
    {
        await _teamRepository.RemoveUser(userId);
        var key = _cacheService.GetUserKey(userId);
        await _cacheService.RemoveData(key);
    }

    public async Task UpdateUser(UpdateUserPayload payload)
    {
        await _teamRepository.UpdateUser(payload);
        var user = await _teamRepository.GetSingleUser(payload.UserId);
        if(user == default) return;
        var key = _cacheService.GetUserKey(user.UserId);
        await _cacheService.SetData(key, user);
    }

    public async Task<UserResponse> GetSingleUser(int userId)
    {
        var key = _cacheService.GetUserKey(userId);
        var user = await _cacheService.GetData<UserResponse>(key);
        if (user == default)
        {
            user = await _teamRepository.GetSingleUser(userId);
            await _cacheService.SetData(key, user);
        }
        return user;
    }

    public async Task<List<UserResponse>> GetUsers()
    {
        var key = _cacheService.GetAllUsersKey();
        var users = await _cacheService.GetData<List<UserResponse>>(key);
        if (!users.Any())
        {
            users = await _teamRepository.GetUsers();
            await _cacheService.SetData(key, users);
        }
        return users;
    }
}