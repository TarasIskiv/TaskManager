using System.Data;
using System.Net.NetworkInformation;
using System.Text;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace TaskManager.Database;

public class DapperContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;
    private readonly string _rootConnectionString;
    private readonly string _databaseName;
    private const string _teamDatabaseName = "TaskManager_Team";
    private const string _taskDatabaseName = "TaskManager_Task";
    public DapperContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _rootConnectionString = configuration.GetConnectionString("DatabaseRoot")!;
        _databaseName = configuration.GetConnectionString("DatabaseName")!;
        _connectionString = _configuration.GetConnectionString("Database")!;
        InitializeDatabase();
    }
    public IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    private void InitializeDatabase()
    {
        using var connection = new SqlConnection(_rootConnectionString);
        var sql = $"""
                   DECLARE @DatabaseExist bit = 1
                   IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = '{_databaseName}')
                   BEGIN
                     SET @DatabaseExist = 0;
                     CREATE DATABASE {_databaseName}
                   END
                   
                   Select @DatabaseExist
                   """;
        var databaseExist = connection.QuerySingle<bool>(sql);
        if (databaseExist) return;
        if (_databaseName.Equals(_taskDatabaseName))
        {
            sql = InitializeTaskTables();
            connection.Execute(sql);
            using var concreteConnection = new SqlConnection(_connectionString);
            sql = InitializeTaskViews();
            concreteConnection.Execute(sql);

        }

        if (_databaseName.Equals(_teamDatabaseName))
        {
            sql = InitializeTeamTables();
            connection.Execute(sql);
            using var concreteConnection = new SqlConnection(_connectionString);
            sql = InitializeTeamViews();
            concreteConnection.Execute(sql);
        }
    }
    private string InitializeTeamTables()
    {
        return $"""
                USE {_databaseName}
                create table UserContactInfo
                (
                    UserId int not null IDENTITY PRIMARY KEY,
                    Email nvarchar(50) not null,
                    Name nvarchar(30) not null,
                    Surname nvarchar(30) not null
                );
                
                create table UserDetails
                (
                    UserDetailsId int not null IDENTITY PRIMARY KEY ,
                    UserId int not null,
                    Role nvarchar(40) not null,
                    DateOfBirth DATETIME2 NULL,
                    Nationality nvarchar(50) null,
                    Salary float null,
                    WorkSince DATETIME2 null
                );
                """;
    }

    private string InitializeTeamViews()
    {
        return $"""
                create view vwUser as
                SELECT
                    UCI.UserId as UserId,
                    UCI.Email,
                    UCI.Name,
                    UCI.Surname,
                    UD.Role,
                    UD.Nationality,
                    UD.Salary,
                    UD.DateOfBirth,
                    UD.WorkSince
                FROM UserContactInfo UCI
                INNER JOIN UserDetails UD on UCI.UserId = UD.UserId;
                """;
    }

    private string InitializeTaskTables()
    {
        return $"""
                USE {_databaseName}
                create table UserContactInfo
                (
                    UserId int not null IDENTITY PRIMARY KEY,
                    Email nvarchar(50) not null,
                    Name nvarchar(30) not null,
                    Surname nvarchar(30) not null
                );
                
                create table Status
                (
                    StatusId int not null IDENTITY primary key,
                    Name nvarchar(30) not null
                );
                
                create table TaskInfo
                (
                    TaskId int not null IDENTITY primary key,
                    Title nvarchar(70) not null,
                    AssignedTo int null,
                    CreationDate DATETIME2 not null,
                    Status int not null
                );
                
                create table TaskDetails
                (
                    TaskDetailsId int not null identity primary key ,
                    TaskId int not null,
                    Description nvarchar(300) null,
                    AcceptanceCriteria nvarchar(300) null,
                    StoryPoints int null,
                    Priority int not null
                );
                
                create table Priority
                (
                    PriorityId int not null IDENTITY PRIMARY KEY ,
                    Name nvarchar(30) not null
                );
                
                insert into Priority
                VALUES
                    ('High'),
                    ('Medium'),
                    ('Low'),
                    ('Lowest')
                
                insert into Status(Name)
                VALUES
                    ('New'),
                    ('Ready'),
                    ('Active'),
                    ('Review'),
                    ('Closed')
                """;
    }

    private string InitializeTaskViews()
    {
        return $"""
                create view vwTask as
                SELECT
                    TI.TaskId,
                    TI.Title,
                    TD.Description,
                    TD.AcceptanceCriteria,
                    S.Name as StatusInfo,
                    UCI.UserId,
                    Case
                        WHEN UCI.Name is null
                            THEN ''
                        ELSE
                        CONCAT(UCI.Name, ' ', UCI.Surname)
                        END as AssignedTo,
                    TD.StoryPoints,
                    P.Name as PriorityInfo,
                    TI.CreationDate
                FROM TaskInfo TI
                INNER JOIN TaskDetails TD on TD.TaskId = TI.TaskId
                LEFT JOIN UserContactInfo UCI on UCI.UserId = TI.AssignedTo
                LEFT JOIN Status S on S.StatusId = TI.Status
                LEFT JOIN Priority P on TD.Priority = P.PriorityId;
                """;
    }
    
}