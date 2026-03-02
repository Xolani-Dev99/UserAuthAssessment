using Microsoft.AspNetCore.Mvc;
using UserAuthApi.DTOs;
using UserAuthApi.Interfaces;

namespace UserAuthApi.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;

    public UsersController(IUserService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var user = await _service.RegisterAsync(request);

        if (user == null)
            return BadRequest("Email already exists.");

        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var (user, token) = await _service.LoginAsync(request);

        if (user == null)
            return Unauthorized("Invalid credentials.");

        return Ok(new { user, token });
    }
}