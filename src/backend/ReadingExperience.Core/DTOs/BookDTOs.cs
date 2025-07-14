namespace ReadingExperience.Core.DTOs;

// Book DTOs
public record BookDto(
    Guid Id,
    string Title,
    string? Subtitle,
    List<string> Authors,
    string? Description,
    string? ISBN10,
    string? ISBN13,
    string? Publisher,
    DateTime? PublishedDate,
    int? PageCount,
    string? Language,
    string? CoverImageUrl,
    List<string> Categories,
    decimal? AverageRating,
    int RatingCount);

public record BookSearchDto(
    string? Query,
    string? Author,
    string? ISBN,
    List<string>? Categories,
    int Page = 1,
    int PageSize = 20);

public record CreateBookDto(
    string Title,
    string? Subtitle,
    List<string> Authors,
    string? Description,
    string? ISBN10,
    string? ISBN13,
    string? Publisher,
    DateTime? PublishedDate,
    int? PageCount,
    string? Language,
    string? CoverImageUrl,
    List<string> Categories);

// External API response DTOs
public record GoogleBookDto(
    string Id,
    string Title,
    List<string>? Authors,
    string? Description,
    string? Publisher,
    string? PublishedDate,
    int? PageCount,
    List<string>? Categories,
    string? Thumbnail,
    List<GoogleBookIdentifier>? IndustryIdentifiers);

public record GoogleBookIdentifier(
    string Type,
    string Identifier);