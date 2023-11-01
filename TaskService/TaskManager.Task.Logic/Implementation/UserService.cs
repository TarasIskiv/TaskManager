using TaskManager.Cache.Abstraction;
using TaskManager.Core.Payloads;
using TaskManager.Task.Logic.Abstraction;
using TaskManager.Task.Repository.Abstraction;

namespace TaskManager.Task.Logic.Implementation;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ICacheService _cacheService;

    public UserService(IUserRepository userRepository, ICacheService cacheService)
    {
        _userRepository = userRepository;
        _cacheService = cacheService;
    }
    public async System.Threading.Tasks.Task CreateUser(int userId, UserContactInfo payload)
    {
        await _userRepository.CreateUser(payload);
        var user = await _userRepository.GetUser(userId);
        
        var key = _cacheService.GetUserKey(userId);
        await _cacheService.SetData(key, user);

        await UpdateCache();
    }

    public async System.Threading.Tasks.Task RemoveUser(int userId)
    {
        await _userRepository.RemoveUser(userId);
        
        var user = await _userRepository.GetUser(userId);
        var key = _cacheService.GetUserKey(userId);
        await _cacheService.RemoveData(key);

        await UpdateCache();
    }

    public async Task<UserContactInfo> GetUser(int userId)
    {
        var key = _cacheService.GetUserKey(userId);
        var user = await _cacheService.GetData<UserContactInfo>(key);
        if (user == default)
        {
            user = await _userRepository.GetUser(userId);
            await _cacheService.SetData(key, user);
        }

        return user;
    }

    public async Task<List<UserContactInfo>> GetUsers()
    {
        var key = _cacheService.GetAllUsersKey();
        var users = await _cacheService.GetData<List<UserContactInfo>>(key);
        if (!users.Any())
        {
            users = await _userRepository.GetUsers();
            await UpdateCache();
        }

        return users;
    }

    private async System.Threading.Tasks.Task UpdateCache()
    {
        var users = await _userRepository.GetUsers();
        var key = _cacheService.GetAllUsersKey();
        await _cacheService.SetData(key, users);
    }
}