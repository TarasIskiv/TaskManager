using StackExchange.Redis;
using TaskManager.Cache.Abstraction;
using TaskManager.Core.Payloads;
using TaskManager.Core.Responses;
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
        var taskId = await _taskRepository.CreateTask(payload);
        if(taskId == default) return;

        var task = await _taskRepository.GetTask(taskId);
        var key = _cacheService.GetTaskKey(taskId);
        await _cacheService.SetData(key, task);
        await UpdateCache();

        if (!string.IsNullOrEmpty(task.AssignedTo))
        {
            //push message to the queue
        }
    }

    public async Task<TaskResponse> GetTask(int taskId)
    {
        var key = _cacheService.GetTaskKey(taskId);
        var task = await _cacheService.GetData<TaskResponse>(key);
        if (task == default)
        {
            task = await _taskRepository.GetTask(taskId);
            await _cacheService.SetData(key, task);
        }
        return task;
    }

    public async Task<List<TaskResponse>> GetTasks()
    {
        var key = _cacheService.GetAllTasksKey();
        var tasks = await _cacheService.GetData<List<TaskResponse>>(key);
        if (!tasks.Any())
        {
            tasks = await _taskRepository.GetTasks();
        }
        await UpdateCache();
        return tasks;
    }

    private System.Threading.Tasks.Task PushMessage()
    {
        throw new NotImplementedException();
    }
    
    private async System.Threading.Tasks.Task UpdateCache()
    {
        var tasks = await _taskRepository.GetTasks();
        var key = _cacheService.GetAllTasksKey();
        await _cacheService.SetData(key, tasks);
    }
}