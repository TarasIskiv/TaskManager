using Dapper;
using TaskManager.Core.Payloads;
using TaskManager.Core.Responses;
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
    public async Task<int> CreateTask(CreateTaskPayload payload)
    {
        string sql = 
            """
                DECLARE @InsertedTaskIds TABLE (TaskId int) 
                
                INSERT INTO TaskInfo(Title, AssignedTo, CreationDate, Status)
                OUTPUT inserted.TaskId INTO @InsertedTaskIds(TaskId)
                VALUES(@Title, @AssignedTo, GetDate(), @Status)
                
                INSERT INTO TaskDetails(TaskId, Priority)
                VALUES(TaskId, @Priority)
                
                SELECT TOP 1 TaskId from @InsertedTaskIds
            """;
        using var connection = _context.CreateConnection();
        var taskId = await connection.QuerySingleAsync<int>(sql, new 
            { 
                Title = payload.Title,
                AssignedTo = payload.AssignedTo,
                Status = payload.Status,
                Priority = payload.Priority
            });
        return taskId;
    }

    public async Task<TaskResponse> GetTask(int taskId)
    {
        string sql = 
            """
                SELECT
                    TaskId,
                    Title,
                    Description,
                    AcceptanceCriteria,
                    StatusInfo,
                    UserId,
                    AssignedTo,
                    StoryPoints,
                    PriorityInfo,
                    CreationDate
                FROM vwTask
                WHERE TaskId = taskId
            """;
        using var connection = _context.CreateConnection();
        var task = await connection.QuerySingleOrDefaultAsync<TaskResponse>(sql, new { TaskId = taskId });
        return task;
    }

    public async Task<List<TaskResponse>> GetTasks()
    {
        string sql = 
            """
                SELECT
                    TaskId,
                    Title,
                    Description,
                    AcceptanceCriteria,
                    StatusInfo,
                    UserId,
                    AssignedTo,
                    StoryPoints,
                    PriorityInfo,
                    CreationDate
                FROM vwTask
            """;
        using var connection = _context.CreateConnection();
        var tasks = await connection.QueryAsync<TaskResponse>(sql);
        return tasks.ToList();
    }
}