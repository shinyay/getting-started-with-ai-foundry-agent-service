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
    public class ReviewsController : ControllerBase
    {
        private readonly BookConnectDbContext _context;
        private readonly ILogger<ReviewsController> _logger;

        public ReviewsController(BookConnectDbContext context, ILogger<ReviewsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Get reviews for a specific book
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviews([FromQuery] Guid? bookId)
        {
            try
            {
                var query = _context.Reviews
                    .Include(r => r.Book)
                    .Include(r => r.User)
                    .AsQueryable();

                if (bookId.HasValue)
                {
                    query = query.Where(r => r.BookId == bookId.Value);
                }

                var reviews = await query
                    .OrderByDescending(r => r.CreatedAt)
                    .Take(50)
                    .Select(r => new ReviewDto
                    {
                        Id = r.Id,
                        UserId = r.UserId,
                        BookId = r.BookId,
                        Rating = r.Rating,
                        Comment = r.Comment,
                        CreatedAt = r.CreatedAt,
                        Book = new BookDto
                        {
                            Id = r.Book.Id,
                            Title = r.Book.Title,
                            Author = r.Book.Author,
                            CoverUrl = r.Book.CoverUrl
                        },
                        User = new UserDto
                        {
                            Id = r.User.Id,
                            Name = r.User.Name,
                            Email = r.User.Email
                        }
                    })
                    .ToListAsync();

                return Ok(reviews);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting reviews for book: {BookId}", bookId);
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
        /// Add a new review
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> AddReview([FromBody] ReviewInput input)
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

                // Check if user already reviewed this book
                var existingReview = await _context.Reviews
                    .FirstOrDefaultAsync(r => r.UserId == user.Id && r.BookId == input.BookId);

                if (existingReview != null)
                {
                    return BadRequest(new ErrorResponse
                    {
                        Error = new ErrorDetail
                        {
                            Code = "ReviewAlreadyExists",
                            Message = "User has already reviewed this book"
                        }
                    });
                }

                var review = new Review
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    BookId = input.BookId,
                    Rating = input.Rating,
                    Comment = input.Comment,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding review");
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