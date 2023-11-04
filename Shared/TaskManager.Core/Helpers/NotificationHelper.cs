using System.Text;
using TaskManager.Core.Responses;

namespace TaskManager.Core.Helpers;

public static class NotificationHelper
{
    public static string GetSubject(bool isAssignedTo, int taskId)
    {
        return $"You are {(isAssignedTo ? "" : "no longer")} assigned to the task #{taskId}";
    }

    public static string GetContext(bool isAssignedTo, TaskResponse task, string userName)
    {
        var messageBuilder = new StringBuilder();
        string header = $"""
                        Hi, {userName}!
                        
                        We want to inform you, that You are {(isAssignedTo ? "" : "no longer")} assigned to the task #{task.TaskId}.
                        """;

        messageBuilder.Append(header);

        if (isAssignedTo)
        {
            var mainContent = $"""
                              Task Details:
                              Task Id: {task.TaskId},
                              Task Title: {task.Title},
                              Task Status: {task.StatusInfo},
                              Task Priority: {task.PriorityInfo}
                              """;
            messageBuilder.Append(mainContent);
        }
        
        string footer = """

                        Best Regards,
                        Task Manager Service.
                        """;
        messageBuilder.Append(footer);
        return messageBuilder.ToString();
    }
}