using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookConnect.Api.Models
{
    public class ReadingRecord
    {
        public Guid Id { get; set; }
        
        [Required]
        public Guid UserId { get; set; }
        
        [Required]
        public Guid BookId { get; set; }
        
        [Range(0, 100)]
        public int Progress { get; set; }
        
        [MaxLength(1000)]
        public string? Note { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
        
        [ForeignKey("BookId")]
        public virtual Book Book { get; set; } = null!;
    }
}