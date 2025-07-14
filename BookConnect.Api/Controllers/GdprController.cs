using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BookConnect.Api.Data;
using BookConnect.Api.DTOs;
using System.Text.Json;

namespace BookConnect.Api.Controllers
{
    [ApiController]
    [Route("api/v1/gdpr")]
    [Authorize]
    public class GdprController : ControllerBase
    {
        private readonly BookConnectDbContext _context;
        private readonly ILogger<GdprController> _logger;

        public GdprController(BookConnectDbContext context, ILogger<GdprController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Request data export for GDPR compliance
        /// </summary>
        [HttpPost("export")]
        public async Task<ActionResult> RequestDataExport()
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

                // In a real implementation, this would:
                // 1. Queue a background job to collect all user data
                // 2. Generate a downloadable file (JSON/CSV)
                // 3. Upload to Azure Blob Storage
                // 4. Send notification email with download link
                // 5. Log the export request for audit purposes

                // For now, we'll simulate the process
                var exportData = await CollectUserData(user.Id);
                
                // In production, you would save this to blob storage and send an email
                // Here we'll just log the request
                _logger.LogInformation("Data export requested for user: {UserId} at {Timestamp}", 
                    user.Id, DateTime.UtcNow);

                return Accepted(new { 
                    Message = "Data export request has been received and is being processed. You will receive an email with the download link within 24 hours." 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing data export request");
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
        /// Request data deletion for GDPR compliance
        /// </summary>
        [HttpPost("delete")]
        public async Task<ActionResult> RequestDataDeletion()
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

                // Implement soft delete - mark user as deleted
                user.DeletedAt = DateTime.UtcNow;
                
                // In a real implementation, you would also:
                // 1. Queue a background job for physical deletion after retention period
                // 2. Anonymize related data that must be kept for business purposes
                // 3. Send confirmation email
                // 4. Log the deletion request for audit purposes

                await _context.SaveChangesAsync();

                _logger.LogInformation("Data deletion requested for user: {UserId} at {Timestamp}", 
                    user.Id, DateTime.UtcNow);

                return Accepted(new { 
                    Message = "Data deletion request has been received and is being processed. Your account will be permanently deleted within 30 days." 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing data deletion request");
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

        private async Task<object> CollectUserData(Guid userId)
        {
            // Collect all user data for export
            var user = await _context.Users.FindAsync(userId);
            var readingRecords = await _context.ReadingRecords
                .Include(rr => rr.Book)
                .Where(rr => rr.UserId == userId)
                .ToListAsync();
            var reviews = await _context.Reviews
                .Include(r => r.Book)
                .Where(r => r.UserId == userId)
                .ToListAsync();
            var ownedBookClubs = await _context.BookClubs
                .Where(bc => bc.OwnerUserId == userId)
                .ToListAsync();
            var bookClubMemberships = await _context.BookClubMembers
                .Include(bcm => bcm.Club)
                .Where(bcm => bcm.UserId == userId)
                .ToListAsync();

            return new
            {
                User = user,
                ReadingRecords = readingRecords,
                Reviews = reviews,
                OwnedBookClubs = ownedBookClubs,
                BookClubMemberships = bookClubMemberships,
                ExportedAt = DateTime.UtcNow
            };
        }
    }
}