using Microsoft.EntityFrameworkCore;
using ReadingExperience.Core.Entities;

namespace ReadingExperience.Infrastructure.Data;

public class ReadingExperienceDbContext : DbContext
{
    public ReadingExperienceDbContext(DbContextOptions<ReadingExperienceDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<ReadingRecord> ReadingRecords { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<ReviewLike> ReviewLikes { get; set; }
    public DbSet<ReviewComment> ReviewComments { get; set; }
    public DbSet<BookClub> BookClubs { get; set; }
    public DbSet<BookClubMembership> BookClubMemberships { get; set; }
    public DbSet<BookClubEvent> BookClubEvents { get; set; }
    public DbSet<EventAttendee> EventAttendees { get; set; }
    public DbSet<BookClubBook> BookClubBooks { get; set; }
    public DbSet<UserFollow> UserFollows { get; set; }
    public DbSet<DiscussionThread> DiscussionThreads { get; set; }
    public DbSet<DiscussionPost> DiscussionPosts { get; set; }
    public DbSet<Recommendation> Recommendations { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Email).HasMaxLength(256).IsRequired();
            entity.Property(e => e.PasswordHash).HasMaxLength(512).IsRequired();
            entity.Property(e => e.Nickname).HasMaxLength(100);
            entity.Property(e => e.Bio).HasMaxLength(1000);
            entity.Property(e => e.AvatarUrl).HasMaxLength(500);
        });

        // Book configuration
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(500).IsRequired();
            entity.Property(e => e.Subtitle).HasMaxLength(500);
            entity.Property(e => e.Authors).HasMaxLength(1000).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(5000);
            entity.Property(e => e.ISBN10).HasMaxLength(10);
            entity.Property(e => e.ISBN13).HasMaxLength(13);
            entity.Property(e => e.Publisher).HasMaxLength(200);
            entity.Property(e => e.Language).HasMaxLength(10);
            entity.Property(e => e.CoverImageUrl).HasMaxLength(500);
            entity.Property(e => e.Categories).HasMaxLength(1000);
            entity.Property(e => e.GoogleBooksId).HasMaxLength(50);
            entity.Property(e => e.OpenLibraryId).HasMaxLength(50);
            entity.HasIndex(e => e.ISBN10);
            entity.HasIndex(e => e.ISBN13);
            entity.HasIndex(e => e.GoogleBooksId);
        });

        // ReadingRecord configuration
        modelBuilder.Entity<ReadingRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.BookId }).IsUnique();
            entity.HasOne(e => e.User)
                  .WithMany(e => e.ReadingRecords)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Book)
                  .WithMany(e => e.ReadingRecords)
                  .HasForeignKey(e => e.BookId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Review configuration
        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.BookId }).IsUnique();
            entity.Property(e => e.Rating).IsRequired();
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.Content).HasMaxLength(5000);
            entity.Property(e => e.Tags).HasMaxLength(1000);
            entity.Property(e => e.ImageUrls).HasMaxLength(2000);
            entity.HasOne(e => e.User)
                  .WithMany(e => e.Reviews)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Book)
                  .WithMany(e => e.Reviews)
                  .HasForeignKey(e => e.BookId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ReviewLike configuration
        modelBuilder.Entity<ReviewLike>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ReviewId, e.UserId }).IsUnique();
            entity.HasOne(e => e.Review)
                  .WithMany(e => e.Likes)
                  .HasForeignKey(e => e.ReviewId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.NoAction);
        });

        // ReviewComment configuration
        modelBuilder.Entity<ReviewComment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Content).HasMaxLength(1000).IsRequired();
            entity.HasOne(e => e.Review)
                  .WithMany(e => e.Comments)
                  .HasForeignKey(e => e.ReviewId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.NoAction);
        });

        // BookClub configuration
        modelBuilder.Entity<BookClub>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.HasOne(e => e.CreatedBy)
                  .WithMany()
                  .HasForeignKey(e => e.CreatedByUserId)
                  .OnDelete(DeleteBehavior.NoAction);
        });

        // BookClubMembership configuration
        modelBuilder.Entity<BookClubMembership>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.BookClubId, e.UserId }).IsUnique();
            entity.HasOne(e => e.BookClub)
                  .WithMany(e => e.Memberships)
                  .HasForeignKey(e => e.BookClubId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.User)
                  .WithMany(e => e.BookClubMemberships)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // UserFollow configuration
        modelBuilder.Entity<UserFollow>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.FollowerId, e.FollowingId }).IsUnique();
            entity.HasOne(e => e.Follower)
                  .WithMany(e => e.Following)
                  .HasForeignKey(e => e.FollowerId)
                  .OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(e => e.Following)
                  .WithMany(e => e.Followers)
                  .HasForeignKey(e => e.FollowingId)
                  .OnDelete(DeleteBehavior.NoAction);
        });

        // Additional configurations for other entities...
        ConfigureAdditionalEntities(modelBuilder);
    }

    private void ConfigureAdditionalEntities(ModelBuilder modelBuilder)
    {
        // BookClubEvent configuration
        modelBuilder.Entity<BookClubEvent>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.Location).HasMaxLength(500);
            entity.Property(e => e.OnlineLink).HasMaxLength(500);
        });

        // EventAttendee configuration
        modelBuilder.Entity<EventAttendee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.EventId, e.UserId }).IsUnique();
        });

        // BookClubBook configuration
        modelBuilder.Entity<BookClubBook>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.BookClubId, e.BookId }).IsUnique();
        });

        // DiscussionThread configuration
        modelBuilder.Entity<DiscussionThread>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Content).HasMaxLength(5000).IsRequired();
        });

        // DiscussionPost configuration
        modelBuilder.Entity<DiscussionPost>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Content).HasMaxLength(5000).IsRequired();
        });

        // Recommendation configuration
        modelBuilder.Entity<Recommendation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Reason).HasMaxLength(1000).IsRequired();
            entity.Property(e => e.RecommendationData).HasMaxLength(5000);
        });

        // Notification configuration
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Type).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Title).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Message).HasMaxLength(1000).IsRequired();
            entity.Property(e => e.Data).HasMaxLength(2000);
        });

        // AuditLog configuration
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Action).HasMaxLength(50).IsRequired();
            entity.Property(e => e.EntityType).HasMaxLength(100).IsRequired();
            entity.Property(e => e.EntityId).HasMaxLength(50);
            entity.Property(e => e.IpAddress).HasMaxLength(45);
            entity.Property(e => e.UserAgent).HasMaxLength(500);
        });
    }
}