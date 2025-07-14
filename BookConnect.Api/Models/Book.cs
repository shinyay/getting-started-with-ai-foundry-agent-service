using System.ComponentModel.DataAnnotations;

namespace BookConnect.Api.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string Author { get; set; } = string.Empty;
        
        [MaxLength(20)]
        public string? ISBN { get; set; }
        
        [MaxLength(512)]
        public string? CoverUrl { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public virtual ICollection<ReadingRecord> ReadingRecords { get; set; } = new List<ReadingRecord>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}