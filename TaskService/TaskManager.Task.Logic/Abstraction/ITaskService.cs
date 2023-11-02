using TaskManager.Core.Payloads;
using TaskManager.Core.Responses;

namespace TaskManager.Task.Logic.Abstraction;

public interface ITaskService
{
    System.Threading.Tasks.Task CreateTask(CreateTaskPayload payload);
    Task<TaskResponse> GetTask(int taskId);
    Task<List<TaskResponse>> GetTasks();

}