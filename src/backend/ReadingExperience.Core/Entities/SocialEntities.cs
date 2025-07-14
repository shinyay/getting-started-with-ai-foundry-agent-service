namespace ReadingExperience.Core.Entities;

public class ReviewLike
{
    public Guid Id { get; set; }
    public Guid ReviewId { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Navigation properties
    public virtual Review Review { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}

public class ReviewComment
{
    public Guid Id { get; set; }
    public Guid ReviewId { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public virtual Review Review { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}

public class UserFollow
{
    public Guid Id { get; set; }
    public Guid FollowerId { get; set; }
    public Guid FollowingId { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Navigation properties
    public virtual User Follower { get; set; } = null!;
    public virtual User Following { get; set; } = null!;
}

public class DiscussionThread
{
    public Guid Id { get; set; }
    public Guid? BookClubId { get; set; }
    public Guid? BookId { get; set; }
    public Guid CreatedByUserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsPinned { get; set; } = false;
    public bool IsLocked { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public virtual BookClub? BookClub { get; set; }
    public virtual Book? Book { get; set; }
    public virtual User CreatedBy { get; set; } = null!;
    public virtual ICollection<DiscussionPost> Posts { get; set; } = new List<DiscussionPost>();
}

public class DiscussionPost
{
    public Guid Id { get; set; }
    public Guid ThreadId { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public virtual DiscussionThread Thread { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}