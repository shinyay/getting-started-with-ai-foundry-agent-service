using ReadingExperience.Core.Entities;
using ReadingExperience.Core.DTOs;

namespace ReadingExperience.Core.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<bool> IsEmailTakenAsync(string email);
}

public interface IBookRepository : IRepository<Book>
{
    Task<IEnumerable<Book>> SearchAsync(BookSearchDto searchCriteria);
    Task<Book?> GetByISBNAsync(string isbn);
    Task<Book?> GetByExternalIdAsync(string externalId, string source);
}

public interface IReadingRecordRepository : IRepository<ReadingRecord>
{
    Task<IEnumerable<ReadingRecord>> GetByUserIdAsync(Guid userId);
    Task<ReadingRecord?> GetByUserAndBookAsync(Guid userId, Guid bookId);
}

public interface IReviewRepository : IRepository<Review>
{
    Task<IEnumerable<Review>> GetByBookIdAsync(Guid bookId, int page, int pageSize);
    Task<IEnumerable<Review>> GetByUserIdAsync(Guid userId, int page, int pageSize);
    Task<bool> HasUserReviewedBookAsync(Guid userId, Guid bookId);
}

public interface IExternalBookService
{
    Task<IEnumerable<GoogleBookDto>> SearchBooksAsync(string query);
    Task<GoogleBookDto?> GetBookByIdAsync(string externalId);
}

public interface IJwtService
{
    string GenerateToken(User user);
    Guid? ValidateToken(string token);
}

public interface IPasswordService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}