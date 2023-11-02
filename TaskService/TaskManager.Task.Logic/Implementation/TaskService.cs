using StackExchange.Redis;
using TaskManager.Cache.Abstraction;
using TaskManager.Core.Payloads;
using TaskManager.MessageBroker.Abstraction;
using TaskManager.Task.Logic.Abstraction;
using TaskManager.Task.Repository.Abstraction;

namespace TaskManager.Task.Logic.Implementation;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ICacheService _cacheService;
    private readonly IQueueService _queueService;

    public TaskService(ITaskRepository taskRepository, ICacheService cacheService, IQueueService queueService)
    {
        _taskRepository = taskRepository;
        _cacheService = cacheService;
        _queueService = queueService;
    }
    public async System.Threading.Tasks.Task CreateTask(CreateTaskPayload payload)
    {
        await _taskRepository.CreateTask(payload);
        
    }

    private System.Threading.Tasks.Task PushMessage()
    {
        throw new NotImplementedException();
    }
    
    private System.Threading.Tasks.Task UpdateCache()
    {
        throw new NotImplementedException();
    }
}