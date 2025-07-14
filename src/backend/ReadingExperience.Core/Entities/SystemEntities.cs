namespace ReadingExperience.Core.Entities;

public class Recommendation
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid BookId { get; set; }
    public decimal Score { get; set; } // AI confidence score 0-1
    public string Reason { get; set; } = string.Empty; // Why this book is recommended
    public string? RecommendationData { get; set; } // JSON data from AI model
    public bool IsAccepted { get; set; } = false;
    public bool IsRejected { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    public DateTime? ViewedAt { get; set; }
    public DateTime? RespondedAt { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Book Book { get; set; } = null!;
}

public class Notification
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Type { get; set; } = string.Empty; // review_like, new_follower, event_reminder, etc.
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Data { get; set; } // JSON data related to notification
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    public DateTime? ReadAt { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
}

public class AuditLog
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public string Action { get; set; } = string.Empty; // CREATE, UPDATE, DELETE, LOGIN, etc.
    public string EntityType { get; set; } = string.Empty; // User, Book, Review, etc.
    public string? EntityId { get; set; }
    public string? OldValues { get; set; } // JSON of old values
    public string? NewValues { get; set; } // JSON of new values
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Navigation properties
    public virtual User? User { get; set; }
}