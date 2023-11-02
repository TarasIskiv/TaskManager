using TaskManager.Core.Payloads;

namespace TaskManager.Task.Logic.Abstraction;

public interface ITaskService
{
    System.Threading.Tasks.Task CreateTask(CreateTaskPayload payload);
    
}