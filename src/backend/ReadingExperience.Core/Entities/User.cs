using ReadingExperience.Core.Enums;

namespace ReadingExperience.Core.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? Nickname { get; set; }
    public string? Bio { get; set; }
    public string? AvatarUrl { get; set; }
    public UserRole Role { get; set; } = UserRole.User;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public virtual ICollection<ReadingRecord> ReadingRecords { get; set; } = new List<ReadingRecord>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    public virtual ICollection<BookClubMembership> BookClubMemberships { get; set; } = new List<BookClubMembership>();
    public virtual ICollection<UserFollow> Followers { get; set; } = new List<UserFollow>();
    public virtual ICollection<UserFollow> Following { get; set; } = new List<UserFollow>();
}