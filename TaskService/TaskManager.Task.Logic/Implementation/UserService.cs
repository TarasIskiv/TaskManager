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
    public async System.Threading.Tasks.Task CreateUser(UserContactInfo payload)
    {
        await _userRepository.CreateUser(payload);
    }

    public async System.Threading.Tasks.Task RemoveUser(int userId)
    {
        await _userRepository.RemoveUser(userId);
    }

    public async Task<UserContactInfo> GetUser(int userId)
    {
        return await _userRepository.GetUser(userId);
    }

    public async Task<List<UserContactInfo>> GetUsers()
    {
        return await _userRepository.GetUsers();
    }
}