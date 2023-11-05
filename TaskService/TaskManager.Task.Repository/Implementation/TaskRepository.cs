using Dapper;
using TaskManager.Core.Enums;
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
                Select TaskId, 1 from @InsertedTaskIds
                
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
                WHERE TaskId = @TaskId
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

    public async System.Threading.Tasks.Task RemoveTask(int taskId)
    {
        string sql = 
            """
                DELETE FROM TaskDetails
                WHERE TaskId = @TaskId;

                DELETE FROM TaskInfo
                where TaskID = @TaskId;
            """;
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(sql, new { TaskId = taskId });
    }

    public async System.Threading.Tasks.Task UpdateTask(UpdateTaskPayload payload)
    {
        string sql = 
            """
                UPDATE TaskInfo
                SET Title = @Title,
                    AssignedTo = @AssignedTo,
                    Status = @Status
                WHERE TaskId = @TaskId
                
                UPDATE TaskDetails
                SET Description = @Description,
                    AcceptanceCriteria = @AcceptanceCriteria,
                    StoryPoints = @StoryPoints,
                    Priority = @Priority
                WHERE TaskId = @TaskId
            """;
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(sql, new
        {
            TaskId = payload.TaskId,
            Title = payload.Title,
            AssignedTo = payload.AssignedTo,
            Status = payload.Status,
            Description = payload.Description,
            AcceptanceCriteria = payload.AcceptanceCriteria,
            StoryPoints = payload.StoryPoints,
            Priority = payload.Priority
        });
    }

    public async Task<List<int>> GetAllUserTasks(int userId)
    {
        string sql =
            """
            SELECT TaskId
            from TaskInfo where UserId = @UserId
            """;

        using var connection = _context.CreateConnection();
        var taskIds = await connection.QueryAsync<int>(sql, new { UserId = userId });
        return taskIds.ToList();
    }

    public async System.Threading.Tasks.Task UpdateAuthorForAllUserTasks(int taskId)
    {
        string sql =
            """
            Update TaskInfo
            Set AssignedTo = 0
            where TaskId = @TaskId
            """;

        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(sql, new { TaskId = taskId });
    }

    public System.Threading.Tasks.Task InitializeDatabase()
    {
        throw new NotImplementedException();
    }
}