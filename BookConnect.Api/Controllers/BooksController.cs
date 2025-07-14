using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BookConnect.Api.Data;
using BookConnect.Api.DTOs;
using BookConnect.Api.Models;

namespace BookConnect.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly BookConnectDbContext _context;
        private readonly ILogger<BooksController> _logger;

        public BooksController(BookConnectDbContext context, ILogger<BooksController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Search for books
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> SearchBooks([FromQuery] string? q)
        {
            try
            {
                var query = _context.Books.AsQueryable();

                if (!string.IsNullOrWhiteSpace(q))
                {
                    query = query.Where(b => 
                        b.Title.Contains(q) || 
                        b.Author.Contains(q) ||
                        (b.ISBN != null && b.ISBN.Contains(q)));
                }

                var books = await query
                    .Take(50) // Limit results
                    .Select(b => new BookDto
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Author = b.Author,
                        CoverUrl = b.CoverUrl
                    })
                    .ToListAsync();

                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching books with query: {Query}", q);
                return StatusCode(500, new ErrorResponse
                {
                    Error = new ErrorDetail
                    {
                        Code = "InternalServerError",
                        Message = "An internal server error occurred"
                    }
                });
            }
        }

        /// <summary>
        /// Get book by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBook(Guid id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);

                if (book == null)
                {
                    return NotFound(new ErrorResponse
                    {
                        Error = new ErrorDetail
                        {
                            Code = "BookNotFound",
                            Message = "Book not found"
                        }
                    });
                }

                var bookDto = new BookDto
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    CoverUrl = book.CoverUrl
                };

                return Ok(bookDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting book with ID: {BookId}", id);
                return StatusCode(500, new ErrorResponse
                {
                    Error = new ErrorDetail
                    {
                        Code = "InternalServerError",
                        Message = "An internal server error occurred"
                    }
                });
            }
        }
    }
}