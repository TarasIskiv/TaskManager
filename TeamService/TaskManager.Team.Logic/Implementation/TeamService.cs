using TaskManager.Cache.Abstraction;
using TaskManager.Core.CustomMapper;
using TaskManager.Core.Enums;
using TaskManager.Core.Payloads;
using TaskManager.Core.QueueConfig;
using TaskManager.Core.Responses;
using TaskManager.MessageBroker.Abstraction;
using TaskManager.Team.Logic.Abstraction;
using TaskManager.Team.Repository.Abstraction;
using TaskManager.Core.CustomMapper;

namespace TaskManager.Team.Logic.Implementation;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly ICacheService _cacheService;
    private readonly IQueueService _queueService;

    public TeamService(ITeamRepository teamRepository, ICacheService cacheService, IQueueService queueService)
    {
        _teamRepository = teamRepository;
        _cacheService = cacheService;
        _queueService = queueService;
    }
    public async Task CreateUser(CreateUserPayload payload)
    {
        var userId = await _teamRepository.CreateUser(payload);
        if(userId == default) return;
        var user = await _teamRepository.GetSingleUser(userId);
        var key = _cacheService.GetUserKey(userId);
        await _cacheService.SetData(key, user);

        await UpdateCache();
        await SendMessage(userId, UserActionType.Create, user);
    }

    public async Task RemoveUser(int userId)
    {
        await _teamRepository.RemoveUser(userId);
        var key = _cacheService.GetUserKey(userId);
        await _cacheService.RemoveData(key);

        await UpdateCache();
        await SendMessage(userId, UserActionType.Remove);
    }

    public async Task UpdateUser(UpdateUserPayload payload)
    {
        await _teamRepository.UpdateUser(payload);
        var user = await _teamRepository.GetSingleUser(payload.UserId);
        if(user == default) return;
        var key = _cacheService.GetUserKey(user.UserId);
        await _cacheService.SetData(key, user);

        await UpdateCache();
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
        if (users is null || !users.Any())
        {
            users = await _teamRepository.GetUsers();
            await UpdateCache();
        }
        return users;
    }

    private async Task UpdateCache()
    {
        var users = await _teamRepository.GetUsers();
        var key = _cacheService.GetAllUsersKey();
        await _cacheService.SetData(key, users);
    }

    private async Task SendMessage(int userId, UserActionType actionType, UserResponse? user = default)
    {
        var queueName = _queueService.GetQueueName(QueueConnection.TaskTeamConnection);
        var messageUser = user == default ? default(UserContactInfo) : user!.Value.MapToUserContactInfo();
        await _queueService.PushMessage(
            new QueueUserMessage()
                { UserId = userId, ActionType = actionType, User = messageUser },
            queueName);
    }
}