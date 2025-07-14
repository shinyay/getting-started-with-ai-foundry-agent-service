using ReadingExperience.Core.Enums;

namespace ReadingExperience.Core.DTOs;

// Reading Record DTOs
public record ReadingRecordDto(
    Guid Id,
    Guid BookId,
    BookDto Book,
    ReadingStatus Status,
    DateTime? StartDate,
    DateTime? EndDate,
    int? CurrentPage,
    string? Notes,
    bool IsFavorite,
    decimal? ProgressPercentage,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public record CreateReadingRecordDto(
    Guid BookId,
    ReadingStatus Status,
    DateTime? StartDate,
    int? CurrentPage,
    string? Notes,
    bool IsFavorite = false);

public record UpdateReadingRecordDto(
    ReadingStatus? Status,
    DateTime? StartDate,
    DateTime? EndDate,
    int? CurrentPage,
    string? Notes,
    bool? IsFavorite);

// Review DTOs
public record ReviewDto(
    Guid Id,
    Guid UserId,
    UserProfileDto User,
    Guid BookId,
    BookDto Book,
    int Rating,
    string? Title,
    string? Content,
    List<string> Tags,
    List<string> ImageUrls,
    bool IsSpoiler,
    int LikeCount,
    bool IsLikedByCurrentUser,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public record CreateReviewDto(
    Guid BookId,
    int Rating,
    string? Title,
    string? Content,
    List<string>? Tags,
    List<string>? ImageUrls,
    bool IsSpoiler = false);

public record UpdateReviewDto(
    int? Rating,
    string? Title,
    string? Content,
    List<string>? Tags,
    List<string>? ImageUrls,
    bool? IsSpoiler);