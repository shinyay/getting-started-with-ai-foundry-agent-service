using System.Text.Json;
using Microsoft.Extensions.Configuration;
using ReadingExperience.Core.DTOs;
using ReadingExperience.Core.Interfaces;

namespace ReadingExperience.Infrastructure.Services;

public class GoogleBooksService : IExternalBookService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _baseUrl = "https://www.googleapis.com/books/v1/volumes";

    public GoogleBooksService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["GoogleBooks:ApiKey"] ?? "";
    }

    public async Task<IEnumerable<GoogleBookDto>> SearchBooksAsync(string query)
    {
        try
        {
            var encodedQuery = Uri.EscapeDataString(query);
            var url = $"{_baseUrl}?q={encodedQuery}&maxResults=20";
            
            if (!string.IsNullOrEmpty(_apiKey))
            {
                url += $"&key={_apiKey}";
            }

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonContent = await response.Content.ReadAsStringAsync();
            var searchResult = JsonSerializer.Deserialize<GoogleBooksSearchResult>(jsonContent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return searchResult?.Items?.Select(ConvertToGoogleBookDto) ?? Enumerable.Empty<GoogleBookDto>();
        }
        catch (Exception)
        {
            // Log error in production
            return Enumerable.Empty<GoogleBookDto>();
        }
    }

    public async Task<GoogleBookDto?> GetBookByIdAsync(string externalId)
    {
        try
        {
            var url = $"{_baseUrl}/{externalId}";
            
            if (!string.IsNullOrEmpty(_apiKey))
            {
                url += $"?key={_apiKey}";
            }

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonContent = await response.Content.ReadAsStringAsync();
            var book = JsonSerializer.Deserialize<GoogleBookItem>(jsonContent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return book != null ? ConvertToGoogleBookDto(book) : null;
        }
        catch (Exception)
        {
            // Log error in production
            return null;
        }
    }

    private GoogleBookDto ConvertToGoogleBookDto(GoogleBookItem item)
    {
        var volumeInfo = item.VolumeInfo;
        
        return new GoogleBookDto(
            Id: item.Id,
            Title: volumeInfo?.Title ?? "Unknown Title",
            Authors: volumeInfo?.Authors ?? new List<string>(),
            Description: volumeInfo?.Description,
            Publisher: volumeInfo?.Publisher,
            PublishedDate: volumeInfo?.PublishedDate,
            PageCount: volumeInfo?.PageCount,
            Categories: volumeInfo?.Categories ?? new List<string>(),
            Thumbnail: volumeInfo?.ImageLinks?.Thumbnail,
            IndustryIdentifiers: volumeInfo?.IndustryIdentifiers?.Select(id => 
                new GoogleBookIdentifier(id.Type, id.Identifier)).ToList()
        );
    }

    // DTOs for Google Books API response
    private class GoogleBooksSearchResult
    {
        public List<GoogleBookItem>? Items { get; set; }
    }

    private class GoogleBookItem
    {
        public string Id { get; set; } = string.Empty;
        public VolumeInfo? VolumeInfo { get; set; }
    }

    private class VolumeInfo
    {
        public string? Title { get; set; }
        public List<string>? Authors { get; set; }
        public string? Description { get; set; }
        public string? Publisher { get; set; }
        public string? PublishedDate { get; set; }
        public int? PageCount { get; set; }
        public List<string>? Categories { get; set; }
        public ImageLinks? ImageLinks { get; set; }
        public List<IndustryIdentifier>? IndustryIdentifiers { get; set; }
    }

    private class ImageLinks
    {
        public string? Thumbnail { get; set; }
    }

    private class IndustryIdentifier
    {
        public string Type { get; set; } = string.Empty;
        public string Identifier { get; set; } = string.Empty;
    }
}