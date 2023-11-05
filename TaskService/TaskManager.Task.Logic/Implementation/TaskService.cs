using StackExchange.Redis;
using TaskManager.Cache.Abstraction;
using TaskManager.Core.Helpers;
using TaskManager.Core.Payloads;
using TaskManager.Core.QueueConfig;
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
    private readonly IUserService _userService;

    public TaskService(ITaskRepository taskRepository, ICacheService cacheService, IQueueService queueService, IUserService userService)
    {
        _taskRepository = taskRepository;
        _cacheService = cacheService;
        _queueService = queueService;
        _userService = userService;
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
            var user = await _userService.GetUser(task.UserId);
            await PushMessage(task, user, true);
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

    public async System.Threading.Tasks.Task RemoveTask(int taskId)
    {
        var task = await _taskRepository.GetTask(taskId);
        if (task == default) return;
        await _taskRepository.RemoveTask(taskId);
        var key = _cacheService.GetTaskKey(taskId);
        await _cacheService.RemoveData(key);
        await UpdateCache();
        if (!string.IsNullOrEmpty(task.AssignedTo))
        {
            var user = await _userService.GetUser(task.UserId);
            await PushMessage(task, user, false);
        }
    }

    public async System.Threading.Tasks.Task UpdateTask(UpdateTaskPayload payload)
    {
        var taskBeforeUpdate = await _taskRepository.GetTask(payload.TaskId);
        if(taskBeforeUpdate == default) return;
        await _taskRepository.UpdateTask(payload);
        var task = await _taskRepository.GetTask(payload.TaskId);

        var key = _cacheService.GetTaskKey(payload.TaskId);
        await _cacheService.SetData(key, task);
        await UpdateCache();

        if (taskBeforeUpdate.AssignedTo != task.AssignedTo)
        {
            var user = await _userService.GetUser(taskBeforeUpdate.UserId);
            await PushMessage(task, user, false);

            user = await _userService.GetUser(task.UserId);
            await PushMessage(task, user, true);
        }
    }

    public async System.Threading.Tasks.Task UpdateAuthorForAllUserTasks(int userId)
    {
        var taskIds = await _taskRepository.GetAllUserTasks(userId);
        foreach (var taskId in taskIds)
        {
            var key = _cacheService.GetTaskKey(taskId);
            await _taskRepository.UpdateAuthorForAllUserTasks(taskId);
            var task = await _taskRepository.GetTask(taskId);
            await _cacheService.SetData(key, task);
        }

        await UpdateCache();
    }

    private async System.Threading.Tasks.Task PushMessage(TaskResponse task, UserContactInfo user, bool isAssigned)
    {
        var queueName = _queueService.GetQueueName(QueueConnection.TaskTeamConnection);
        await _queueService.PushMessage(
            new QueueNotificationMessage()
            {
                Context = NotificationHelper.GetContext(isAssigned, task, user.Name) ,
                ReceiverEmail = user.Email,
                ReceiverFullName = string.Concat(user.Name, " ", user.Surname),
                Subject = NotificationHelper.GetSubject(isAssigned, task.TaskId)
            },
            queueName);
    }
    
    private async System.Threading.Tasks.Task UpdateCache()
    {
        var tasks = await _taskRepository.GetTasks();
        var key = _cacheService.GetAllTasksKey();
        await _cacheService.SetData(key, tasks);
    }
}