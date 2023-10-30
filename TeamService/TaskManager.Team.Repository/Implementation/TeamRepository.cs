using Dapper;
using TaskManager.Core.Payloads;
using TaskManager.Core.Responses;
using TaskManager.Database;
using TaskManager.Team.Repository.Abstraction;

namespace TaskManager.Team.Repository.Implementation;

public class TeamRepository : ITeamRepository
{
    private readonly DapperContext _context;

    public TeamRepository(DapperContext context)
    {
        _context = context;
    }
    public async Task<int> CreateUser(CreateUserPayload payload)
    {
        string sql = 
            """
            IF NOT EXISTS (SELECT * FROM UserContactInfo WHERE Email = '@Email' )
                BEGIN
                    DECLARE @InsertedUserId int
            
                    INSERT INTO UserContactInfo (Email, Name, Surname)
                    OUTPUT @InsertedUserId = inserted.UserId
                    VALUES (@Email, @Name, @Surname)
                    
                    INSERT INTO UserDetails(UserId, Role, DateOfBirth, Nationality, Salary, WorkSince)
                    VALUES (@InsertedUserId, @Role, @DateOfBirth, @Nationality, @Salary, @WorkSince)
                    
                    Return @InsertedUserId
                END
            ELSE
                BEGIN  
                    Return 0;
                END      
            """;

        using var connection = _context.CreateConnection();
        var userId = await connection.ExecuteAsync(sql, new
        {
            Email = payload.Email,
            Name = payload.Name,
            Surname = payload.Surname,
            Role = payload.Role,
            DateOfBirth = payload.DateOfBirth,
            Nationality = payload.Nationality,
            Salary = payload.Salary,
            WorkSince = payload.WorkSince
        });

        return userId;
    }

    public async Task RemoveUser(int userId)
    {
        string sql = 
            """
            DELETE FROM UserDetails where UserId = @UserId
            Delete FROM UserContactInfo where UserId = @UserId
            """;
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(sql, new {UserId = userId});
    }

    public async Task UpdateUser(UpdateUserPayload payload)
    {
        string sql =
            """
            Update UserDetails
            Set Role = @Role,
                DateOfBirth = @DateOfBirth,
                Nationality = @Nationality,
                Salary = @Salary,
                WorkSince = @WorkSince
            WHERE UserId = @UserId
            """;
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(sql, new
        {
            UserId = payload.UserId,
            Role = payload.Role,
            DateOfBirth = payload.DateOfBirth,
            Nationality = payload.Nationality,
            Salary = payload.Salary,
            WorkSince = payload.WorkSince
        });
    }

    public async Task<UserResponse> GetSingleUser(int userId)
    {
        string sql =
            """
            select 
                UserId,
                Email,
                Name,
                Surname,
                Role,
                DateOfBirth,
                Salary,
                Nationality,
                WorkSince
            from vwUser
            where UserId = @UserId
            """;
        using var connection = _context.CreateConnection();
        return await connection.QuerySingleAsync<UserResponse>(sql, new { UserId = userId });
    }

    public async Task<List<UserResponse>> GetUsers()
    {
        string sql =
            """
            select
                UserId,
                Email,
                Name,
                Surname,
                Role,
                DateOfBirth,
                Salary,
                Nationality,
                WorkSince
            from vwUser
            """;
        using var connection = _context.CreateConnection();
        var users = await connection.QueryAsync<UserResponse>(sql);
        return users.ToList();
    }
}