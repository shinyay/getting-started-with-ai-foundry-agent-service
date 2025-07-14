using ReadingExperience.Core.Enums;

namespace ReadingExperience.Core.DTOs;

// User DTOs
public record UserRegistrationDto(
    string Email,
    string Password,
    string? Nickname);

public record UserLoginDto(
    string Email,
    string Password);

public record UserProfileDto(
    Guid Id,
    string Email,
    string? Nickname,
    string? Bio,
    string? AvatarUrl,
    DateTime CreatedAt);

public record UpdateUserProfileDto(
    string? Nickname,
    string? Bio,
    string? AvatarUrl);

// Authentication DTOs
public record AuthenticationResponseDto(
    string Token,
    UserProfileDto User);

public record ChangePasswordDto(
    string CurrentPassword,
    string NewPassword);