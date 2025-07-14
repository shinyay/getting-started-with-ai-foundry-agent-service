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
    public class BookclubsController : ControllerBase
    {
        private readonly BookConnectDbContext _context;
        private readonly ILogger<BookclubsController> _logger;

        public BookclubsController(BookConnectDbContext context, ILogger<BookclubsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Get all book clubs
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookClubDto>>> GetBookClubs()
        {
            try
            {
                var bookClubs = await _context.BookClubs
                    .Include(bc => bc.OwnerUser)
                    .Include(bc => bc.Members)
                    .ThenInclude(m => m.User)
                    .OrderByDescending(bc => bc.CreatedAt)
                    .Select(bc => new BookClubDto
                    {
                        Id = bc.Id,
                        Name = bc.Name,
                        Description = bc.Description,
                        OwnerUserId = bc.OwnerUserId,
                        ScheduledAt = bc.ScheduledAt,
                        CreatedAt = bc.CreatedAt,
                        Owner = new UserDto
                        {
                            Id = bc.OwnerUser.Id,
                            Name = bc.OwnerUser.Name,
                            Email = bc.OwnerUser.Email
                        },
                        Members = bc.Members.Select(m => new UserDto
                        {
                            Id = m.User.Id,
                            Name = m.User.Name,
                            Email = m.User.Email
                        }).ToList()
                    })
                    .ToListAsync();

                return Ok(bookClubs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting book clubs");
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
        /// Create a new book club
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> CreateBookClub([FromBody] BookClubInput input)
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

                var bookClub = new BookClub
                {
                    Id = Guid.NewGuid(),
                    Name = input.Name,
                    Description = input.Description,
                    OwnerUserId = user.Id,
                    ScheduledAt = input.ScheduledAt,
                    CreatedAt = DateTime.UtcNow
                };

                _context.BookClubs.Add(bookClub);

                // Add the creator as the first member
                var membership = new BookClubMember
                {
                    ClubId = bookClub.Id,
                    UserId = user.Id,
                    JoinedAt = DateTime.UtcNow
                };

                _context.BookClubMembers.Add(membership);
                await _context.SaveChangesAsync();

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating book club");
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
        /// Join a book club
        /// </summary>
        [HttpPost("{id}/join")]
        public async Task<ActionResult> JoinBookClub(Guid id)
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

                var bookClub = await _context.BookClubs.FindAsync(id);
                if (bookClub == null)
                {
                    return NotFound(new ErrorResponse
                    {
                        Error = new ErrorDetail
                        {
                            Code = "BookClubNotFound",
                            Message = "Book club not found"
                        }
                    });
                }

                // Check if already a member
                var existingMembership = await _context.BookClubMembers
                    .FirstOrDefaultAsync(m => m.ClubId == id && m.UserId == user.Id);

                if (existingMembership != null)
                {
                    return BadRequest(new ErrorResponse
                    {
                        Error = new ErrorDetail
                        {
                            Code = "AlreadyMember",
                            Message = "User is already a member of this book club"
                        }
                    });
                }

                var membership = new BookClubMember
                {
                    ClubId = id,
                    UserId = user.Id,
                    JoinedAt = DateTime.UtcNow
                };

                _context.BookClubMembers.Add(membership);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error joining book club: {BookClubId}", id);
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