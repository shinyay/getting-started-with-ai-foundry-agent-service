using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BookConnect.Api.Data;
using BookConnect.Api.DTOs;
using BookConnect.Api.Models;

namespace BookConnect.Api.Controllers
{
    [ApiController]
    [Route("api/v1/reading-records")]
    [Authorize]
    public class ReadingRecordsController : ControllerBase
    {
        private readonly BookConnectDbContext _context;
        private readonly ILogger<ReadingRecordsController> _logger;

        public ReadingRecordsController(BookConnectDbContext context, ILogger<ReadingRecordsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Get reading records for current user
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadingRecordDto>>> GetReadingRecords()
        {
            try
            {
                var userEmail = User.Identity?.Name ?? "user@example.com";
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

                if (user == null)
                {
                    return NotFound(new ErrorResponse
                    {
                        Error = new ErrorDetail
                        {
                            Code = "UserNotFound",
                            Message = "User not found"
                        }
                    });
                }

                var readingRecords = await _context.ReadingRecords
                    .Include(rr => rr.Book)
                    .Where(rr => rr.UserId == user.Id)
                    .OrderByDescending(rr => rr.CreatedAt)
                    .Select(rr => new ReadingRecordDto
                    {
                        Id = rr.Id,
                        UserId = rr.UserId,
                        BookId = rr.BookId,
                        Progress = rr.Progress,
                        Note = rr.Note,
                        CreatedAt = rr.CreatedAt,
                        Book = new BookDto
                        {
                            Id = rr.Book.Id,
                            Title = rr.Book.Title,
                            Author = rr.Book.Author,
                            CoverUrl = rr.Book.CoverUrl
                        }
                    })
                    .ToListAsync();

                return Ok(readingRecords);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting reading records");
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
        /// Add a new reading record
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> AddReadingRecord([FromBody] ReadingRecordInput input)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userEmail = User.Identity?.Name ?? "user@example.com";
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

                if (user == null)
                {
                    return NotFound(new ErrorResponse
                    {
                        Error = new ErrorDetail
                        {
                            Code = "UserNotFound",
                            Message = "User not found"
                        }
                    });
                }

                // Check if book exists
                var book = await _context.Books.FindAsync(input.BookId);
                if (book == null)
                {
                    return BadRequest(new ErrorResponse
                    {
                        Error = new ErrorDetail
                        {
                            Code = "BookNotFound",
                            Message = "Book not found"
                        }
                    });
                }

                var readingRecord = new ReadingRecord
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    BookId = input.BookId,
                    Progress = input.Progress,
                    Note = input.Note,
                    CreatedAt = DateTime.UtcNow
                };

                _context.ReadingRecords.Add(readingRecord);
                await _context.SaveChangesAsync();

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding reading record");
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