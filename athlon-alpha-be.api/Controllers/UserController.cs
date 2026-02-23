using athlon_alpha_be.api.Services;

using Microsoft.AspNetCore.Mvc;

namespace athlon_alpha_be.api.Controllers;

[ApiController]
[Route("api/users")]
public class UserController(IUserService _userService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetAllUsersAsync()
    {
        return Ok(await _userService.GetUsersAsync());
    }

    [HttpGet("{id:Guid}")]
    public async Task<ActionResult> GetUserAsync(Guid id)
    {
        return Ok(await _userService.GetUserByIdAsync(id));
    }
}
