using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BookConnect.Api.Data;
using BookConnect.Api.DTOs;

namespace BookConnect.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class RecommendationsController : ControllerBase
    {
        private readonly BookConnectDbContext _context;
        private readonly ILogger<RecommendationsController> _logger;

        public RecommendationsController(BookConnectDbContext context, ILogger<RecommendationsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Get AI-powered book recommendations for the current user
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetRecommendations()
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

                // Get user's reading history and preferences
                var userReadingHistory = await _context.ReadingRecords
                    .Include(rr => rr.Book)
                    .Where(rr => rr.UserId == user.Id)
                    .OrderByDescending(rr => rr.CreatedAt)
                    .Take(10)
                    .ToListAsync();

                var userReviews = await _context.Reviews
                    .Include(r => r.Book)
                    .Where(r => r.UserId == user.Id && r.Rating >= 4)
                    .OrderByDescending(r => r.CreatedAt)
                    .Take(5)
                    .ToListAsync();

                // Simple recommendation algorithm:
                // 1. Find books by the same authors the user has rated highly
                // 2. Find books with similar titles/genres (basic text matching)
                // 3. Return books the user hasn't read yet

                var favoriteAuthors = userReviews
                    .Select(r => r.Book.Author)
                    .Distinct()
                    .ToList();

                var readBookIds = userReadingHistory
                    .Select(rr => rr.BookId)
                    .ToHashSet();

                var recommendations = await _context.Books
                    .Where(b => 
                        favoriteAuthors.Contains(b.Author) && 
                        !readBookIds.Contains(b.Id))
                    .Take(10)
                    .Select(b => new BookDto
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Author = b.Author,
                        CoverUrl = b.CoverUrl
                    })
                    .ToListAsync();

                // If no recommendations from favorite authors, return popular books
                if (!recommendations.Any())
                {
                    recommendations = await _context.Books
                        .Where(b => !readBookIds.Contains(b.Id))
                        .OrderByDescending(b => b.CreatedAt)
                        .Take(10)
                        .Select(b => new BookDto
                        {
                            Id = b.Id,
                            Title = b.Title,
                            Author = b.Author,
                            CoverUrl = b.CoverUrl
                        })
                        .ToListAsync();
                }

                return Ok(recommendations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recommendations");
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