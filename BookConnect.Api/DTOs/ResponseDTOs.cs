namespace BookConnect.Api.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
    
    public class BookDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string? CoverUrl { get; set; }
    }
    
    public class ReadingRecordDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        public int Progress { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedAt { get; set; }
        public BookDto? Book { get; set; }
    }
    
    public class ReviewDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public BookDto? Book { get; set; }
        public UserDto? User { get; set; }
    }
    
    public class BookClubDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid OwnerUserId { get; set; }
        public DateTime ScheduledAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserDto? Owner { get; set; }
        public List<UserDto> Members { get; set; } = new List<UserDto>();
    }
    
    public class ErrorResponse
    {
        public ErrorDetail Error { get; set; } = new ErrorDetail();
    }
    
    public class ErrorDetail
    {
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}