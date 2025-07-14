namespace ReadingExperience.Core.Entities;

public class Review
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid BookId { get; set; }
    public int Rating { get; set; } // 1-5 stars
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? Tags { get; set; } // JSON array of tags
    public string? ImageUrls { get; set; } // JSON array of image URLs
    public bool IsSpoiler { get; set; } = false;
    public int LikeCount { get; set; } = 0;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Book Book { get; set; } = null!;
    public virtual ICollection<ReviewLike> Likes { get; set; } = new List<ReviewLike>();
    public virtual ICollection<ReviewComment> Comments { get; set; } = new List<ReviewComment>();
}