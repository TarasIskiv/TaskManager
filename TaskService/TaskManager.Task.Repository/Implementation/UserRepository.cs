using Azure.Core;
using Dapper;
using TaskManager.Core.Payloads;
using TaskManager.Database;
using TaskManager.Task.Repository.Abstraction;

namespace TaskManager.Task.Repository.Implementation;

public class UserRepository : IUserRepository
{
    private readonly DapperContext _context;

    public UserRepository(DapperContext context)
    {
        _context = context;
    }
    public async Task<int> CreateUser(UserContactInfo payload)
    {
        string sql =
            """
            DECLARE @UserId int
            INSERT Into UserContactInfo (Email, Name, Surname)
            Output @Userid = inserted.UserId
            VALUES (@Email, @Name, @Surname)
            
            Return @UserId
            """;
        using var connection = _context.CreateConnection();
        return await connection.QuerySingleAsync<int>(sql, new { Email = payload.Email, Name = payload.Name, Surname = payload.Surname });
    }

    public async System.Threading.Tasks.Task RemoveUser(int userId)
    {
        string sql =
            """
            DELETE FROM UserContactInfo WHERE UserId = @UserId
            """;
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(sql, new { UserId = userId });
    }

    public async Task<UserContactInfo> GetUser(int userId)
    {
        string sql =
            """
            select
                UserId,
                Email,
                Name,
                Surname
            from UserContactInfo
            where UserId = @UserId
            """;
        using var connection = _context.CreateConnection();
        return await connection.QuerySingleAsync<UserContactInfo>(sql, new { UserId = userId });
    }

    public async Task<List<UserContactInfo>> GetUsers()
    {
        string sql =
            """
            select
                UserId,
                Email,
                Name,
                Surname
            from UserContactInfo
            """;
        using var connection = _context.CreateConnection();
        var users = await connection.QueryAsync<UserContactInfo>(sql);
        return users.ToList();
    }
}