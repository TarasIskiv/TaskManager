using Dapper;
using TaskManager.Core.Payloads;
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
    public async Task<int> CreateUser(UpsertUserPayload payload)
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
}