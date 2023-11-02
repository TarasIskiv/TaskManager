using TaskManager.Core.Payloads;
using TaskManager.Core.Responses;

namespace TaskManager.Task.Repository.Abstraction;

public interface ITaskRepository
{
    System.Threading.Tasks.Task CreateTask(CreateTaskPayload payload);
    Task<TaskResponse> GetTask(int taskId);

}