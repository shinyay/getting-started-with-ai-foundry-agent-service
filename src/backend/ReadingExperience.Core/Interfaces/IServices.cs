using ReadingExperience.Core.DTOs;

namespace ReadingExperience.Core.Interfaces;

public interface IUserService
{
    Task<AuthenticationResponseDto> RegisterAsync(UserRegistrationDto request);
    Task<AuthenticationResponseDto> LoginAsync(UserLoginDto request);
    Task<UserProfileDto> GetProfileAsync(Guid userId);
    Task<UserProfileDto> UpdateProfileAsync(Guid userId, UpdateUserProfileDto request);
    Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDto request);
    Task<bool> DeleteAccountAsync(Guid userId);
}

public interface IBookService
{
    Task<IEnumerable<BookDto>> SearchBooksAsync(BookSearchDto searchCriteria);
    Task<BookDto?> GetBookByIdAsync(Guid bookId);
    Task<BookDto> CreateBookAsync(CreateBookDto request);
    Task<IEnumerable<BookDto>> SearchExternalBooksAsync(string query);
    Task<BookDto> ImportBookFromExternalAsync(string externalId, string source);
}

public interface IReadingRecordService
{
    Task<IEnumerable<ReadingRecordDto>> GetUserReadingRecordsAsync(Guid userId);
    Task<ReadingRecordDto?> GetReadingRecordAsync(Guid userId, Guid bookId);
    Task<ReadingRecordDto> CreateOrUpdateReadingRecordAsync(Guid userId, CreateReadingRecordDto request);
    Task<ReadingRecordDto> UpdateReadingRecordAsync(Guid userId, Guid recordId, UpdateReadingRecordDto request);
    Task<bool> DeleteReadingRecordAsync(Guid userId, Guid recordId);
}

public interface IReviewService
{
    Task<IEnumerable<ReviewDto>> GetBookReviewsAsync(Guid bookId, int page = 1, int pageSize = 20);
    Task<IEnumerable<ReviewDto>> GetUserReviewsAsync(Guid userId, int page = 1, int pageSize = 20);
    Task<ReviewDto?> GetReviewAsync(Guid reviewId);
    Task<ReviewDto> CreateReviewAsync(Guid userId, CreateReviewDto request);
    Task<ReviewDto> UpdateReviewAsync(Guid userId, Guid reviewId, UpdateReviewDto request);
    Task<bool> DeleteReviewAsync(Guid userId, Guid reviewId);
    Task<bool> LikeReviewAsync(Guid userId, Guid reviewId);
    Task<bool> UnlikeReviewAsync(Guid userId, Guid reviewId);
}