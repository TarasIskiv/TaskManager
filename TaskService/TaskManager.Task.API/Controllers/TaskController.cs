using Microsoft.AspNetCore.Mvc;
using TaskManager.Core.Payloads;
using TaskManager.Task.Logic.Abstraction;

namespace TaskManager.Task.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpPost("CreateTask")]
    public async Task<IActionResult> CreateTask(CreateTaskPayload payload)
    {
        await _taskService.CreateTask(payload);
        return Ok();
    }

    [HttpGet("GetTask")]
    public async Task<IActionResult> GetTask([FromQuery] int taskId)
    {
        var task = await _taskService.GetTask(taskId);
        return Ok(task);
    }
    
    [HttpGet("GetTasks")]
    public async Task<IActionResult> GetTasks()
    {
        var tasks = await _taskService.GetTasks();
        return Ok(tasks);
    }

    [HttpDelete("RemoveTask")]
    public async Task<IActionResult> RemoveTask([FromQuery] int taskId)
    {
        await _taskService.RemoveTask(taskId);
        return Ok();
    }
}