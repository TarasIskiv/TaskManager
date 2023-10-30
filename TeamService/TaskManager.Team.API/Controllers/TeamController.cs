using Microsoft.AspNetCore.Mvc;
using TaskManager.Core.Payloads;
using TaskManager.Team.Logic.Abstraction;

namespace TaskManager.Team.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamController : ControllerBase
{
    private readonly ITeamService _teamService;

    public TeamController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserPayload payload)
    {
        await _teamService.CreateUser(payload);
        return Ok();
    }

    [HttpDelete("RemoveUser")]
    public async Task<IActionResult> RemoveUser([FromQuery] int userId)
    {
        return Ok();
    }

    [HttpPut("UpdateUser")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserPayload payload)
    {
        await _teamService.UpdateUser(payload);
        return Ok();
    }
}