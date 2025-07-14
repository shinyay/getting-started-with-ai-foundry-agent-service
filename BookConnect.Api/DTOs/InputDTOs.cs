using System.ComponentModel.DataAnnotations;

namespace BookConnect.Api.DTOs
{
    public class ReadingRecordInput
    {
        [Required]
        public Guid BookId { get; set; }
        
        [Range(0, 100)]
        public int Progress { get; set; }
        
        [MaxLength(1000)]
        public string? Note { get; set; }
    }
    
    public class ReviewInput
    {
        [Required]
        public Guid BookId { get; set; }
        
        [Range(1, 5)]
        public int Rating { get; set; }
        
        [MaxLength(2000)]
        public string? Comment { get; set; }
    }
    
    public class BookClubInput
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(1000)]
        public string? Description { get; set; }
        
        public DateTime ScheduledAt { get; set; }
    }
}