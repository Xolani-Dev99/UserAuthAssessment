using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserAuthApi.Data;
using UserAuthApi.DTOs;
using UserAuthApi.Interfaces;
using UserAuthApi.Models;

namespace UserAuthApi.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher<User> _hasher;
    private readonly IJwtService _jwt;
    private readonly ILogger<UserService> _logger;

    public UserService(
        ApplicationDbContext context,
        IPasswordHasher<User> hasher,
        IJwtService jwt,
        ILogger<UserService> logger)
    {
        _context = context;
        _hasher = hasher;
        _jwt = jwt;
        _logger = logger;
    }

    public async Task<UserResponse?> RegisterAsync(RegisterRequest request)
    {
        var email = request.Email.ToLower().Trim();

        if (await _context.Users.AnyAsync(x => x.Email == email))
        {
            _logger.LogWarning("Registration failed. Email already exists: {Email}", email);
            return null;
        }

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = email
        };

        user.PasswordHash = _hasher.HashPassword(user, request.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        _logger.LogInformation("User registered successfully: {Email}", email);

        return new UserResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
    }

    public async Task<(UserResponse? user, string? token)> LoginAsync(LoginRequest request)
    {
        var email = request.Email.ToLower().Trim();

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
            return (null, null);

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (result == PasswordVerificationResult.Failed)
            return (null, null);

        var token = _jwt.GenerateToken(user);

        return (
            new UserResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            },
            token
        );
    }
}