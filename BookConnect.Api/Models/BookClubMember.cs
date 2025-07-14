using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookConnect.Api.Models
{
    public class BookClubMember
    {
        [Required]
        public Guid ClubId { get; set; }
        
        [Required]
        public Guid UserId { get; set; }
        
        public DateTime JoinedAt { get; set; }
        
        // Navigation properties
        [ForeignKey("ClubId")]
        public virtual BookClub Club { get; set; } = null!;
        
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
}