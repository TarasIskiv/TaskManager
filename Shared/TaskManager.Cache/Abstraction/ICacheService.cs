namespace TaskManager.Cache.Abstraction;

public interface ICacheService
{
    string GetAllUsersKey();
    string GetAllTasksKey();
    string GetUserKey(int userId);
    string GetTaskKey(int taskId);
    Task<T> GetData<T>(string key);
    Task SetData<T>(string key, T data);
}