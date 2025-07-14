using System.Text.Json;
using ReadingExperience.Core.DTOs;
using ReadingExperience.Core.Entities;
using ReadingExperience.Core.Interfaces;

namespace ReadingExperience.Core.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IExternalBookService _externalBookService;

    public BookService(IBookRepository bookRepository, IExternalBookService externalBookService)
    {
        _bookRepository = bookRepository;
        _externalBookService = externalBookService;
    }

    public async Task<IEnumerable<BookDto>> SearchBooksAsync(BookSearchDto searchCriteria)
    {
        var books = await _bookRepository.SearchAsync(searchCriteria);
        return books.Select(MapToBookDto);
    }

    public async Task<BookDto?> GetBookByIdAsync(Guid bookId)
    {
        var book = await _bookRepository.GetByIdAsync(bookId);
        return book != null ? MapToBookDto(book) : null;
    }

    public async Task<BookDto> CreateBookAsync(CreateBookDto request)
    {
        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Subtitle = request.Subtitle,
            Authors = JsonSerializer.Serialize(request.Authors),
            Description = request.Description,
            ISBN10 = request.ISBN10,
            ISBN13 = request.ISBN13,
            Publisher = request.Publisher,
            PublishedDate = request.PublishedDate,
            PageCount = request.PageCount,
            Language = request.Language,
            CoverImageUrl = request.CoverImageUrl,
            Categories = JsonSerializer.Serialize(request.Categories),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _bookRepository.AddAsync(book);
        return MapToBookDto(book);
    }

    public async Task<IEnumerable<BookDto>> SearchExternalBooksAsync(string query)
    {
        var externalBooks = await _externalBookService.SearchBooksAsync(query);
        return externalBooks.Select(ConvertFromGoogleBookDto);
    }

    public async Task<BookDto> ImportBookFromExternalAsync(string externalId, string source)
    {
        // Check if book already exists
        var existingBook = await _bookRepository.GetByExternalIdAsync(externalId, source);
        if (existingBook != null)
        {
            return MapToBookDto(existingBook);
        }

        // Fetch from external service
        var externalBook = await _externalBookService.GetBookByIdAsync(externalId);
        if (externalBook == null)
        {
            throw new InvalidOperationException("Book not found in external service");
        }

        // Extract ISBN information
        var isbn10 = externalBook.IndustryIdentifiers?.FirstOrDefault(x => x.Type == "ISBN_10")?.Identifier;
        var isbn13 = externalBook.IndustryIdentifiers?.FirstOrDefault(x => x.Type == "ISBN_13")?.Identifier;

        // Create new book
        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = externalBook.Title,
            Authors = JsonSerializer.Serialize(externalBook.Authors ?? new List<string>()),
            Description = externalBook.Description,
            ISBN10 = isbn10,
            ISBN13 = isbn13,
            Publisher = externalBook.Publisher,
            PublishedDate = TryParseDate(externalBook.PublishedDate),
            PageCount = externalBook.PageCount,
            Categories = JsonSerializer.Serialize(externalBook.Categories ?? new List<string>()),
            CoverImageUrl = externalBook.Thumbnail,
            GoogleBooksId = source.ToLower() == "google" ? externalId : null,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _bookRepository.AddAsync(book);
        return MapToBookDto(book);
    }

    private static BookDto MapToBookDto(Book book)
    {
        var authors = TryDeserializeStringList(book.Authors);
        var categories = TryDeserializeStringList(book.Categories);

        return new BookDto(
            Id: book.Id,
            Title: book.Title,
            Subtitle: book.Subtitle,
            Authors: authors,
            Description: book.Description,
            ISBN10: book.ISBN10,
            ISBN13: book.ISBN13,
            Publisher: book.Publisher,
            PublishedDate: book.PublishedDate,
            PageCount: book.PageCount,
            Language: book.Language,
            CoverImageUrl: book.CoverImageUrl,
            Categories: categories,
            AverageRating: book.AverageRating,
            RatingCount: book.RatingCount
        );
    }

    private static BookDto ConvertFromGoogleBookDto(GoogleBookDto googleBook)
    {
        return new BookDto(
            Id: Guid.Empty, // This will be set when imported
            Title: googleBook.Title,
            Subtitle: null,
            Authors: googleBook.Authors ?? new List<string>(),
            Description: googleBook.Description,
            ISBN10: googleBook.IndustryIdentifiers?.FirstOrDefault(x => x.Type == "ISBN_10")?.Identifier,
            ISBN13: googleBook.IndustryIdentifiers?.FirstOrDefault(x => x.Type == "ISBN_13")?.Identifier,
            Publisher: googleBook.Publisher,
            PublishedDate: TryParseDate(googleBook.PublishedDate),
            PageCount: googleBook.PageCount,
            Language: null,
            CoverImageUrl: googleBook.Thumbnail,
            Categories: googleBook.Categories ?? new List<string>(),
            AverageRating: null,
            RatingCount: 0
        );
    }

    private static List<string> TryDeserializeStringList(string? json)
    {
        if (string.IsNullOrEmpty(json))
            return new List<string>();

        try
        {
            return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
        }
        catch
        {
            return new List<string> { json };
        }
    }

    private static DateTime? TryParseDate(string? dateString)
    {
        if (string.IsNullOrEmpty(dateString))
            return null;

        // Try parsing various date formats
        var formats = new[] { "yyyy-MM-dd", "yyyy-MM", "yyyy" };
        
        foreach (var format in formats)
        {
            if (DateTime.TryParseExact(dateString, format, null, System.Globalization.DateTimeStyles.None, out var date))
            {
                return date;
            }
        }

        if (DateTime.TryParse(dateString, out var parsedDate))
        {
            return parsedDate;
        }

        return null;
    }
}