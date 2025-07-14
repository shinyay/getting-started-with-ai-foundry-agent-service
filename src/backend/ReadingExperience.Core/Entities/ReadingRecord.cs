using ReadingExperience.Core.Enums;

namespace ReadingExperience.Core.Entities;

public class ReadingRecord
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid BookId { get; set; }
    public ReadingStatus Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? CurrentPage { get; set; }
    public string? Notes { get; set; }
    public bool IsFavorite { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Progress calculation
    public decimal? ProgressPercentage 
    { 
        get 
        {
            if (CurrentPage.HasValue && Book?.PageCount.HasValue == true && Book.PageCount > 0)
                return (decimal)CurrentPage.Value / Book.PageCount.Value * 100;
            return null;
        }
    }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Book Book { get; set; } = null!;
}