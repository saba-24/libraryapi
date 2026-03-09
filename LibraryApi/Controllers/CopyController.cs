using LibraryApi.Data.ApiResponse;
using LibraryApi.Data.Dto;
using LibraryApi.Data.Entities;
using LibraryApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CopyController(ICopyService copyService, IUserService userService) : Controller
{
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAsync()
    {
        try
        {
            return Ok(ApiResponse.Success("fetched all books", await copyService.GetAsync()));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse.Fail("server error", e.GetType().ToString()));
        }
    }

    [HttpGet]
    [Authorize]
    [Route("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute] string id)
    {
        try
        {
            return Ok(ApiResponse.Success("fetched book", await copyService.GetByIdAsync(id)));
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
            var result = await copyService.GetPagedAsync(page, pageSize);
            return Ok(ApiResponse.Success("fetched book data", result));
        }
        catch (Exception e)
        {
            return StatusCode(500, ApiResponse.Fail(e.Message, "server error"));
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddAsync([FromBody] CopyDto copy)
    {
        try
        {
            await copyService.AddAsync(copy);
            return Ok(ApiResponse.Success("added book", null));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse.Fail("server error", e.GetType().ToString()));
        }
    }

    [HttpPut]
    [Authorize]
    [Route("{id}")]
    public async Task<IActionResult> UpdateAsync([FromBody] CopyDto copy, [FromRoute] string id)
    {
        try
        {
            await copyService.UpdateAsync(id, copy);
            return Ok(ApiResponse.Success("updated book", await copyService.GetByIdAsync(id)));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse.Fail("server error", e.GetType().ToString()));
        }
    }

    [HttpPut]
    [Authorize]
    [Route("borrow")]
    public async Task<IActionResult> UpdateBorrowerAsync([FromQuery] string userId, [FromQuery] string copyId)
    {
        try
        {
            User u = await userService.GetByIdAsync(userId);
            BookCopy copy = await copyService.GetByIdAsync(copyId);
            if (u.BorrowCount > 4)
                return BadRequest(
                    ApiResponse.Fail("user cannot borrow any more books since they reached the borrow limit", null));
            if (u.BorrowedBooks.Contains(copy.ISBN))
                return BadRequest(ApiResponse.Fail("user already borrowed this book", null));
            await userService.BorrowAsync(copy.ISBN, userId);
            await copyService.UpdateBorrowerAsync(userId, copyId);
            return Ok(ApiResponse.Success("borrowed book", null));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse.Fail("server error", e.GetType().ToString()));
        }
    }

    [HttpPut]
    [Authorize]
    [Route("return")]
    public async Task<IActionResult> ReturnAsync([FromQuery] string userId, [FromQuery] string copyId)
    {
        try
        {
            User u = await userService.GetByIdAsync(userId);
            BookCopy copy = await copyService.GetByIdAsync(copyId);
            if (!u.BorrowedBooks.Contains(copy.ISBN))
                return BadRequest(ApiResponse.Fail("user has not borrowed this book", null));
            await userService.ReturnAsync(copy.ISBN, userId);
            await copyService.ReturnBookAsync(copyId);
            return Ok(ApiResponse.Success("returned book", null));
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
            await copyService.DeleteAsync(id);
            return Ok(ApiResponse.Success("removed book copy", null));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse.Fail("server error", e.GetType().ToString()));
        }
    }
}