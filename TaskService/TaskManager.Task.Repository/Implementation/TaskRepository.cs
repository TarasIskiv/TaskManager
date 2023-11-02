using Dapper;
using TaskManager.Core.Payloads;
using TaskManager.Database;
using TaskManager.Task.Repository.Abstraction;

namespace TaskManager.Task.Repository.Implementation;

public class TaskRepository : ITaskRepository
{
    private readonly DapperContext _context;

    public TaskRepository(DapperContext context)
    {
        _context = context;
    }
    public async System.Threading.Tasks.Task CreateTask(CreateTaskPayload payload)
    {
        string sql = 
            """
                DECLARE @InsertedTaskIds TABLE (TaskId int) 
                
                INSERT INTO TaskInfo(Title, AssignedTo, CreationDate, Status)
                OUTPUT inserted.TaskId INTO @InsertedTaskIds(TaskId)
                VALUES(@Title, @AssignedTo, GetDate(), @Status)
                
                INSERT INTO TaskDetails(TaskId, Priority)
                VALUES(TaskId, @Priority)
            """;
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(sql, new 
            { 
                Title = payload.Title,
                AssignedTo = payload.AssignedTo,
                Status = payload.Status,
                Priority = payload.Priority
            });
    }
}