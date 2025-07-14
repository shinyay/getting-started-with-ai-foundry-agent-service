using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookConnect.Api.Models
{
    public class Review
    {
        public Guid Id { get; set; }
        
        [Required]
        public Guid UserId { get; set; }
        
        [Required]
        public Guid BookId { get; set; }
        
        [Range(1, 5)]
        public int Rating { get; set; }
        
        [MaxLength(2000)]
        public string? Comment { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
        
        [ForeignKey("BookId")]
        public virtual Book Book { get; set; } = null!;
    }
}