namespace ReadingExperience.Core.Entities;

public class BookClub
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid CreatedByUserId { get; set; }
    public int MaxMembers { get; set; } = 50;
    public bool IsPrivate { get; set; } = false;
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public virtual User CreatedBy { get; set; } = null!;
    public virtual ICollection<BookClubMembership> Memberships { get; set; } = new List<BookClubMembership>();
    public virtual ICollection<BookClubEvent> Events { get; set; } = new List<BookClubEvent>();
    public virtual ICollection<BookClubBook> Books { get; set; } = new List<BookClubBook>();
    public virtual ICollection<DiscussionThread> DiscussionThreads { get; set; } = new List<DiscussionThread>();
}

public class BookClubMembership
{
    public Guid Id { get; set; }
    public Guid BookClubId { get; set; }
    public Guid UserId { get; set; }
    public DateTime JoinedAt { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public virtual BookClub BookClub { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}

public class BookClubEvent
{
    public Guid Id { get; set; }
    public Guid BookClubId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? Location { get; set; }
    public string? OnlineLink { get; set; }
    public int MaxAttendees { get; set; } = 50;
    public DateTime CreatedAt { get; set; }
    
    // Navigation properties
    public virtual BookClub BookClub { get; set; } = null!;
    public virtual ICollection<EventAttendee> Attendees { get; set; } = new List<EventAttendee>();
}

public class EventAttendee
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public DateTime RegisteredAt { get; set; }
    public bool IsAttending { get; set; } = true;
    
    // Navigation properties
    public virtual BookClubEvent Event { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}

public class BookClubBook
{
    public Guid Id { get; set; }
    public Guid BookClubId { get; set; }
    public Guid BookId { get; set; }
    public DateTime AddedAt { get; set; }
    public bool IsCurrentlyReading { get; set; } = false;
    
    // Navigation properties
    public virtual BookClub BookClub { get; set; } = null!;
    public virtual Book Book { get; set; } = null!;
}