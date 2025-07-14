using System.ComponentModel.DataAnnotations;

namespace BookConnect.Api.Models
{
    public class User
    {
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;
        
        [MaxLength(256)]
        public string? PasswordHash { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? DeletedAt { get; set; }
        
        // Navigation properties
        public virtual ICollection<ReadingRecord> ReadingRecords { get; set; } = new List<ReadingRecord>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<BookClub> OwnedBookClubs { get; set; } = new List<BookClub>();
        public virtual ICollection<BookClubMember> BookClubMemberships { get; set; } = new List<BookClubMember>();
    }
}