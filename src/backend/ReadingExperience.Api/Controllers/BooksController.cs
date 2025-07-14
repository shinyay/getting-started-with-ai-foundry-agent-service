using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadingExperience.Core.DTOs;
using ReadingExperience.Core.Interfaces;

namespace ReadingExperience.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    /// <summary>
    /// Search books in the local database
    /// </summary>
    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<BookDto>>> SearchBooks([FromQuery] BookSearchDto searchCriteria)
    {
        try
        {
            var books = await _bookService.SearchBooksAsync(searchCriteria);
            return Ok(books);
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An error occurred while searching books" });
        }
    }

    /// <summary>
    /// Get a specific book by ID
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<BookDto>> GetBook(Guid id)
    {
        try
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound(new { message = "Book not found" });
            }
            return Ok(book);
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the book" });
        }
    }

    /// <summary>
    /// Search books from external sources (Google Books API)
    /// </summary>
    [HttpGet("external/search")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<BookDto>>> SearchExternalBooks([FromQuery] string query)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest(new { message = "Query parameter is required" });
            }

            var books = await _bookService.SearchExternalBooksAsync(query);
            return Ok(books);
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An error occurred while searching external books" });
        }
    }

    /// <summary>
    /// Import a book from external source and add to local database
    /// </summary>
    [HttpPost("import")]
    public async Task<ActionResult<BookDto>> ImportBook([FromBody] ImportBookRequest request)
    {
        try
        {
            var book = await _bookService.ImportBookFromExternalAsync(request.ExternalId, request.Source);
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An error occurred while importing the book" });
        }
    }

    /// <summary>
    /// Add a new book manually
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<BookDto>> CreateBook([FromBody] CreateBookDto request)
    {
        try
        {
            var book = await _bookService.CreateBookAsync(request);
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An error occurred while creating the book" });
        }
    }
}

public record ImportBookRequest(string ExternalId, string Source);