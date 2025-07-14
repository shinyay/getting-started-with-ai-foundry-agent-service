using BookConnect.Api.Data;
using BookConnect.Api.Models;

namespace BookConnect.Api.Data
{
    public static class SeedData
    {
        public static async Task Initialize(BookConnectDbContext context)
        {
            // Check if database has been seeded
            if (context.Users.Any())
            {
                return; // Database has been seeded
            }

            // Create sample users
            var users = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "John Doe",
                    Email = "user@example.com",
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Jane Smith",
                    Email = "jane@example.com",
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Alice Johnson",
                    Email = "alice@example.com",
                    CreatedAt = DateTime.UtcNow
                }
            };

            context.Users.AddRange(users);

            // Create sample books
            var books = new List<Book>
            {
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "The Great Gatsby",
                    Author = "F. Scott Fitzgerald",
                    ISBN = "9780743273565",
                    CoverUrl = "https://example.com/covers/gatsby.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "To Kill a Mockingbird",
                    Author = "Harper Lee",
                    ISBN = "9780061120084",
                    CoverUrl = "https://example.com/covers/mockingbird.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "1984",
                    Author = "George Orwell",
                    ISBN = "9780451524935",
                    CoverUrl = "https://example.com/covers/1984.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "Pride and Prejudice",
                    Author = "Jane Austen",
                    ISBN = "9780141040349",
                    CoverUrl = "https://example.com/covers/pride.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "The Catcher in the Rye",
                    Author = "J.D. Salinger",
                    ISBN = "9780316769174",
                    CoverUrl = "https://example.com/covers/catcher.jpg",
                    CreatedAt = DateTime.UtcNow
                }
            };

            context.Books.AddRange(books);

            // Save to get the IDs
            await context.SaveChangesAsync();

            // Create sample reading records
            var readingRecords = new List<ReadingRecord>
            {
                new ReadingRecord
                {
                    Id = Guid.NewGuid(),
                    UserId = users[0].Id,
                    BookId = books[0].Id,
                    Progress = 75,
                    Note = "Really enjoying this classic!",
                    CreatedAt = DateTime.UtcNow
                },
                new ReadingRecord
                {
                    Id = Guid.NewGuid(),
                    UserId = users[0].Id,
                    BookId = books[1].Id,
                    Progress = 100,
                    Note = "Finished reading. Excellent book!",
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                }
            };

            context.ReadingRecords.AddRange(readingRecords);

            // Create sample reviews
            var reviews = new List<Review>
            {
                new Review
                {
                    Id = Guid.NewGuid(),
                    UserId = users[0].Id,
                    BookId = books[1].Id,
                    Rating = 5,
                    Comment = "A timeless classic that everyone should read!",
                    CreatedAt = DateTime.UtcNow.AddDays(-3)
                },
                new Review
                {
                    Id = Guid.NewGuid(),
                    UserId = users[1].Id,
                    BookId = books[2].Id,
                    Rating = 4,
                    Comment = "Disturbing but brilliantly written dystopian novel.",
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                }
            };

            context.Reviews.AddRange(reviews);

            // Create sample book club
            var bookClub = new BookClub
            {
                Id = Guid.NewGuid(),
                Name = "Classic Literature Club",
                Description = "A book club focused on reading and discussing classic literature",
                OwnerUserId = users[0].Id,
                ScheduledAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };

            context.BookClubs.Add(bookClub);
            await context.SaveChangesAsync();

            // Add members to book club
            var bookClubMembers = new List<BookClubMember>
            {
                new BookClubMember
                {
                    ClubId = bookClub.Id,
                    UserId = users[0].Id,
                    JoinedAt = DateTime.UtcNow
                },
                new BookClubMember
                {
                    ClubId = bookClub.Id,
                    UserId = users[1].Id,
                    JoinedAt = DateTime.UtcNow.AddDays(-1)
                }
            };

            context.BookClubMembers.AddRange(bookClubMembers);

            await context.SaveChangesAsync();
        }
    }
}