using TaskManager.Core.Payloads;

namespace TaskManager.Task.Repository.Abstraction;

public interface ITaskRepository
{
    System.Threading.Tasks.Task CreateTask(CreateTaskPayload payload);
}