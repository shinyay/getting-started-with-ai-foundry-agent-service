namespace ReadingExperience.Core.Entities;

public class Book
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public string Authors { get; set; } = string.Empty; // JSON array of authors
    public string? Description { get; set; }
    public string? ISBN10 { get; set; }
    public string? ISBN13 { get; set; }
    public string? Publisher { get; set; }
    public DateTime? PublishedDate { get; set; }
    public int? PageCount { get; set; }
    public string? Language { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? Categories { get; set; } // JSON array of categories
    public decimal? AverageRating { get; set; }
    public int RatingCount { get; set; } = 0;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // External API identifiers
    public string? GoogleBooksId { get; set; }
    public string? OpenLibraryId { get; set; }
    
    // Navigation properties
    public virtual ICollection<ReadingRecord> ReadingRecords { get; set; } = new List<ReadingRecord>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    public virtual ICollection<BookClubBook> BookClubBooks { get; set; } = new List<BookClubBook>();
}