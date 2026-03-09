using LibraryApi.Data.ApiResponse;
using LibraryApi.Data.Dto;
using LibraryApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService) : Controller
{
    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAsync()
    {
        try
        {
            return Ok(ApiResponse.Success("fetched user data", await userService.GetAsync()));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse.Fail("server error", e.GetType().ToString()));
        }
    }

    [HttpGet]
    [Authorize]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAsync([FromRoute] string id)
    {
        try
        {
            return Ok(ApiResponse.Success($"fetched user with id {id}", await userService.GetByIdAsync(id)));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse.Fail("server error", e.GetType().ToString()));
        }
    }

    [HttpGet]
    [Authorize]
    [Route("page")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPageAsync([FromQuery] int page, [FromQuery] int pageSize)
    {
        try
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest(ApiResponse.Fail(null, "page or page size is less than 1"));
            var result = await userService.GetPagedAsync(page, pageSize);
            return Ok(ApiResponse.Success("fetched user data", result));
        }
        catch (Exception e)
        {
            return StatusCode(500, ApiResponse.Fail(e.Message, "server error"));
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> PostAsync([FromBody] UserDto user)
    {
        try
        {
            await userService.AddAsync(user);
            return Ok(ApiResponse.Success("added user", "no data"));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse.Fail("server error", e.GetType().ToString()));
        }
    }

    [HttpPut]
    [Authorize]
    [Route("{id}")]
    public async Task<IActionResult> PutAsync([FromRoute] string id, [FromBody] UserDto user)
    {
        try
        {
            await userService.UpdateAsync(id, user);
            return Ok(ApiResponse.Success("updated user", await userService.GetByIdAsync(id)));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse.Fail("server error", e.GetType().ToString()));
        }
    }

    [HttpDelete]
    [Authorize]
    [Route("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] string id)
    {
        try
        {
            await userService.DeleteAsync(id);
            return Ok(ApiResponse.Success("user deleted", "no data"));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse.Fail("server error", e.GetType().ToString()));
        }
    }
}