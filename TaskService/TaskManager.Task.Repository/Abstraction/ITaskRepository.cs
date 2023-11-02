using TaskManager.Core.Payloads;
using TaskManager.Core.Responses;

namespace TaskManager.Task.Repository.Abstraction;

public interface ITaskRepository
{
    Task<int> CreateTask(CreateTaskPayload payload);
    Task<TaskResponse> GetTask(int taskId);
    Task<List<TaskResponse>> GetTasks();
    System.Threading.Tasks.Task RemoveTask(int taskId);

}