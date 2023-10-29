using System.Text.Json;
using TaskManager.Cache.Abstraction;
using Microsoft.Extensions.Caching.Distributed;

namespace TaskManager.Cache.Implementation;

public class CacheService : ICacheService
{
    private IDistributedCache _cache;
    private DistributedCacheEntryOptions _options { get; set; } = new();

    public CacheService(IDistributedCache distributedCache)
    {
        _cache = distributedCache;
        _options.AbsoluteExpirationRelativeToNow = new TimeSpan(1, 0, 0);
    }
    
    public string GetAllUsersKey() => "UserList";
    public string GetAllTasksKey() => "TaskList";
    public string GetUserKey(int userId) => $"Users/{userId}";
    public string GetTaskKey(int taskId) => $"Tasks/{taskId}";

    public async Task<T> GetData<T>(string key)
    {
        var data = await _cache.GetStringAsync(key);
        var desirializedData = JsonSerializer.Deserialize<T>(data);
        return desirializedData ?? default!;
    }

    public async Task SetData<T>(string key, T data)
    {
        await _cache.RemoveAsync(key);
        var serializedData = JsonSerializer.Serialize(data);
        await _cache.SetStringAsync(key, serializedData, _options);
    }
}