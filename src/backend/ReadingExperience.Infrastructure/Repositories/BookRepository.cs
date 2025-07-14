using Microsoft.EntityFrameworkCore;
using ReadingExperience.Core.Entities;
using ReadingExperience.Core.Interfaces;
using ReadingExperience.Core.DTOs;
using ReadingExperience.Infrastructure.Data;

namespace ReadingExperience.Infrastructure.Repositories;

public class BookRepository : BaseRepository<Book>, IBookRepository
{
    public BookRepository(ReadingExperienceDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Book>> SearchAsync(BookSearchDto searchCriteria)
    {
        var query = _dbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchCriteria.Query))
        {
            query = query.Where(b => 
                b.Title.Contains(searchCriteria.Query) ||
                b.Authors.Contains(searchCriteria.Query) ||
                (b.Description != null && b.Description.Contains(searchCriteria.Query)));
        }

        if (!string.IsNullOrWhiteSpace(searchCriteria.Author))
        {
            query = query.Where(b => b.Authors.Contains(searchCriteria.Author));
        }

        if (!string.IsNullOrWhiteSpace(searchCriteria.ISBN))
        {
            query = query.Where(b => b.ISBN10 == searchCriteria.ISBN || b.ISBN13 == searchCriteria.ISBN);
        }

        if (searchCriteria.Categories != null && searchCriteria.Categories.Any())
        {
            foreach (var category in searchCriteria.Categories)
            {
                query = query.Where(b => b.Categories != null && b.Categories.Contains(category));
            }
        }

        return await query
            .Skip((searchCriteria.Page - 1) * searchCriteria.PageSize)
            .Take(searchCriteria.PageSize)
            .ToListAsync();
    }

    public async Task<Book?> GetByISBNAsync(string isbn)
    {
        return await _dbSet.FirstOrDefaultAsync(b => b.ISBN10 == isbn || b.ISBN13 == isbn);
    }

    public async Task<Book?> GetByExternalIdAsync(string externalId, string source)
    {
        return source.ToLower() switch
        {
            "google" => await _dbSet.FirstOrDefaultAsync(b => b.GoogleBooksId == externalId),
            "openlibrary" => await _dbSet.FirstOrDefaultAsync(b => b.OpenLibraryId == externalId),
            _ => null
        };
    }
}