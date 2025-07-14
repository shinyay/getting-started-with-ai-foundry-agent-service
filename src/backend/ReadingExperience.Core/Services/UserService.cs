using ReadingExperience.Core.DTOs;
using ReadingExperience.Core.Entities;
using ReadingExperience.Core.Enums;
using ReadingExperience.Core.Interfaces;

namespace ReadingExperience.Core.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;

    public UserService(
        IUserRepository userRepository,
        IPasswordService passwordService,
        IJwtService jwtService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _jwtService = jwtService;
    }

    public async Task<AuthenticationResponseDto> RegisterAsync(UserRegistrationDto request)
    {
        // Check if email is already taken
        if (await _userRepository.IsEmailTakenAsync(request.Email))
        {
            throw new InvalidOperationException("Email is already registered");
        }

        // Create new user
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = _passwordService.HashPassword(request.Password),
            Nickname = request.Nickname,
            Role = UserRole.User,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true
        };

        await _userRepository.AddAsync(user);

        // Generate JWT token
        var token = _jwtService.GenerateToken(user);

        return new AuthenticationResponseDto(
            Token: token,
            User: MapToUserProfileDto(user)
        );
    }

    public async Task<AuthenticationResponseDto> LoginAsync(UserLoginDto request)
    {
        // Find user by email
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null || !user.IsActive)
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        // Verify password
        if (!_passwordService.VerifyPassword(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        // Generate JWT token
        var token = _jwtService.GenerateToken(user);

        return new AuthenticationResponseDto(
            Token: token,
            User: MapToUserProfileDto(user)
        );
    }

    public async Task<UserProfileDto> GetProfileAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }

        return MapToUserProfileDto(user);
    }

    public async Task<UserProfileDto> UpdateProfileAsync(Guid userId, UpdateUserProfileDto request)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }

        // Update fields
        if (request.Nickname != null)
            user.Nickname = request.Nickname;
        if (request.Bio != null)
            user.Bio = request.Bio;
        if (request.AvatarUrl != null)
            user.AvatarUrl = request.AvatarUrl;

        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);

        return MapToUserProfileDto(user);
    }

    public async Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDto request)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        // Verify current password
        if (!_passwordService.VerifyPassword(request.CurrentPassword, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Current password is incorrect");
        }

        // Update password
        user.PasswordHash = _passwordService.HashPassword(request.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);
        return true;
    }

    public async Task<bool> DeleteAccountAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        // Soft delete by marking as inactive
        user.IsActive = false;
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);
        return true;
    }

    private static UserProfileDto MapToUserProfileDto(User user)
    {
        return new UserProfileDto(
            Id: user.Id,
            Email: user.Email,
            Nickname: user.Nickname,
            Bio: user.Bio,
            AvatarUrl: user.AvatarUrl,
            CreatedAt: user.CreatedAt
        );
    }
}