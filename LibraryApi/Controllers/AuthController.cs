using LibraryApi.Data.ApiResponse;
using LibraryApi.Data.Auth;
using LibraryApi.Data.Dto;
using LibraryApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : Controller
{
    [HttpGet]
    [Route("login")]
    public async Task<IActionResult> LoginAsync([FromBody] ApiUserDto u)
    {
        try
        {
            string? token = await authService.LoginAsync(u.Username, u.Password);
            if (token == null) return BadRequest();
            return Ok(ApiResponse.Success("logged in", token));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse.Fail("server error", e.GetType().ToString()));
        }
    }
    
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] ApiUserDto u)
    {
        try
        {
            bool success = await authService.RegisterAsync(u);
            return Ok(ApiResponse.Success("signed up", u));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse.Fail("server error", e.GetType().ToString()));
        }
    }
}