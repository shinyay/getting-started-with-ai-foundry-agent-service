using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookConnect.Api.Models
{
    public class BookClub
    {
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(1000)]
        public string? Description { get; set; }
        
        [Required]
        public Guid OwnerUserId { get; set; }
        
        public DateTime ScheduledAt { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        [ForeignKey("OwnerUserId")]
        public virtual User OwnerUser { get; set; } = null!;
        
        public virtual ICollection<BookClubMember> Members { get; set; } = new List<BookClubMember>();
    }
}