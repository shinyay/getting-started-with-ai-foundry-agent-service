using Microsoft.EntityFrameworkCore;
using BookConnect.Api.Models;

namespace BookConnect.Api.Data
{
    public class BookConnectDbContext : DbContext
    {
        public BookConnectDbContext(DbContextOptions<BookConnectDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<ReadingRecord> ReadingRecords { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<BookClub> BookClubs { get; set; }
        public DbSet<BookClubMember> BookClubMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");
            });

            // Configure Book entity
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");
            });

            // Configure ReadingRecord entity
            modelBuilder.Entity<ReadingRecord>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");
                
                entity.HasOne(e => e.User)
                    .WithMany(u => u.ReadingRecords)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.Book)
                    .WithMany(b => b.ReadingRecords)
                    .HasForeignKey(e => e.BookId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Review entity
            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.BookId);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");
                
                entity.HasOne(e => e.User)
                    .WithMany(u => u.Reviews)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.Book)
                    .WithMany(b => b.Reviews)
                    .HasForeignKey(e => e.BookId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure BookClub entity
            modelBuilder.Entity<BookClub>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");
                
                entity.HasOne(e => e.OwnerUser)
                    .WithMany(u => u.OwnedBookClubs)
                    .HasForeignKey(e => e.OwnerUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure BookClubMember entity
            modelBuilder.Entity<BookClubMember>(entity =>
            {
                entity.HasKey(e => new { e.ClubId, e.UserId });
                entity.Property(e => e.JoinedAt).HasDefaultValueSql("datetime('now')");
                
                entity.HasOne(e => e.Club)
                    .WithMany(c => c.Members)
                    .HasForeignKey(e => e.ClubId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.User)
                    .WithMany(u => u.BookClubMemberships)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}