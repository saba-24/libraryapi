using LibraryApi.Data.ApiResponse;
using LibraryApi.Data.Dto;
using LibraryApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController(IBookService bookService) : Controller
{
    /*public async Task<IActionResult> GetAsync()
    {
        try
        {
            return Ok(ApiResponse.Success());
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse.Fail("server error", e.GetType().ToString()));
        }
    }*/
    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAsync()
    {
        try
        {
            return Ok(ApiResponse.Success("fetched book data", await bookService.GetAsync()));
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
            return Ok(ApiResponse.Success($"fetched book with id {id}", await bookService.GetByIdAsync(id)));
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
            var result = await bookService.GetPagedAsync(page, pageSize);
            return Ok(ApiResponse.Success("fetched book data", result));
        }
        catch (Exception e)
        {
            return StatusCode(500, ApiResponse.Fail(e.Message, "server error"));
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> PostAsync([FromBody] BookDto book)
    {
        try
        {
            await bookService.AddAsync(book);
            return Ok(ApiResponse.Success("added book", "no data"));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse.Fail("server error", e.GetType().ToString()));
        }
    }

    [HttpPut]
    [Authorize]
    [Route("{id}")]
    public async Task<IActionResult> PutAsync([FromRoute] string id, [FromBody] BookDto book)
    {
        try
        {
            await bookService.UpdateAsync(id, book);
            return Ok(ApiResponse.Success("updated book", await bookService.GetByIdAsync(id)));
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
            await bookService.DeleteAsync(id);
            return Ok(ApiResponse.Success("book deleted", "no data"));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse.Fail("server error", e.GetType().ToString()));
        }
    }
}