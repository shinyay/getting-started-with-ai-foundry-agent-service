import React, { useState, useEffect } from 'react';
import { Book } from '../types/api';
import { booksApi } from '../services/api';
import './BooksPage.css';

const BooksPage: React.FC = () => {
  const [books, setBooks] = useState<Book[]>([]);
  const [searchQuery, setSearchQuery] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [searchType, setSearchType] = useState<'local' | 'external'>('local');
  const [error, setError] = useState('');

  useEffect(() => {
    // Load initial books
    loadBooks();
  }, []);

  const loadBooks = async () => {
    setIsLoading(true);
    setError('');
    try {
      const results = await booksApi.search({});
      setBooks(results);
    } catch (err) {
      setError('Failed to load books');
    } finally {
      setIsLoading(false);
    }
  };

  const handleSearch = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!searchQuery.trim()) return;

    setIsLoading(true);
    setError('');
    
    try {
      let results: Book[];
      if (searchType === 'local') {
        results = await booksApi.search({ query: searchQuery });
      } else {
        results = await booksApi.searchExternal(searchQuery);
      }
      setBooks(results);
    } catch (err) {
      setError('Search failed. Please try again.');
    } finally {
      setIsLoading(false);
    }
  };

  const handleImportBook = async (book: Book) => {
    try {
      if (book.id) {
        // Book already exists in local database
        return;
      }
      
      // This would be a Google Books book, import it
      await booksApi.import({
        externalId: book.id || '',
        source: 'google'
      });
      
      // Refresh the search to show updated results
      await handleSearch(new Event('submit') as any);
    } catch (err) {
      setError('Failed to import book');
    }
  };

  return (
    <div className="books-page">
      <div className="books-container">
        <div className="books-header">
          <h1>Discover Books</h1>
          <p>Search through our library or discover new books online</p>
        </div>

        <div className="search-section">
          <form onSubmit={handleSearch} className="search-form">
            <div className="search-input-group">
              <input
                type="text"
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
                placeholder="Search for books, authors, or ISBN..."
                className="search-input"
                disabled={isLoading}
              />
              <button 
                type="submit" 
                className="search-button"
                disabled={isLoading}
              >
                {isLoading ? '🔄' : '🔍'}
              </button>
            </div>
            
            <div className="search-options">
              <label className="radio-option">
                <input
                  type="radio"
                  value="local"
                  checked={searchType === 'local'}
                  onChange={(e) => setSearchType('local')}
                />
                My Library
              </label>
              <label className="radio-option">
                <input
                  type="radio"
                  value="external"
                  checked={searchType === 'external'}
                  onChange={(e) => setSearchType('external')}
                />
                Discover Online
              </label>
            </div>
          </form>
        </div>

        {error && (
          <div className="error-message">
            {error}
          </div>
        )}

        <div className="books-grid">
          {isLoading ? (
            <div className="loading-state">
              <div className="loading-spinner"></div>
              <p>Searching for books...</p>
            </div>
          ) : books.length === 0 ? (
            <div className="empty-state">
              <div className="empty-icon">📚</div>
              <h3>No books found</h3>
              <p>
                {searchQuery 
                  ? `No results for "${searchQuery}". Try a different search term.`
                  : 'Start by searching for your favorite books!'}
              </p>
            </div>
          ) : (
            books.map((book) => (
              <div key={book.id || book.title} className="book-card">
                <div className="book-cover">
                  {book.coverImageUrl ? (
                    <img src={book.coverImageUrl} alt={book.title} />
                  ) : (
                    <div className="book-cover-placeholder">
                      📖
                    </div>
                  )}
                </div>
                
                <div className="book-info">
                  <h3 className="book-title">{book.title}</h3>
                  {book.subtitle && (
                    <p className="book-subtitle">{book.subtitle}</p>
                  )}
                  <p className="book-authors">
                    by {book.authors.join(', ')}
                  </p>
                  
                  {book.publishedDate && (
                    <p className="book-year">
                      Published: {new Date(book.publishedDate).getFullYear()}
                    </p>
                  )}
                  
                  {book.pageCount && (
                    <p className="book-pages">{book.pageCount} pages</p>
                  )}
                  
                  {book.averageRating && (
                    <div className="book-rating">
                      ⭐ {book.averageRating.toFixed(1)} ({book.ratingCount} reviews)
                    </div>
                  )}
                  
                  <div className="book-actions">
                    {searchType === 'external' && !book.id ? (
                      <button 
                        onClick={() => handleImportBook(book)}
                        className="btn btn-primary btn-sm"
                      >
                        Add to Library
                      </button>
                    ) : (
                      <button className="btn btn-secondary btn-sm">
                        View Details
                      </button>
                    )}
                  </div>
                </div>
              </div>
            ))
          )}
        </div>
      </div>
    </div>
  );
};

export default BooksPage;