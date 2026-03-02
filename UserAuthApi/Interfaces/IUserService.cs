using UserAuthApi.DTOs;

namespace UserAuthApi.Interfaces;

public interface IUserService
{
    Task<UserResponse?> RegisterAsync(RegisterRequest request);
    Task<(UserResponse? user, string? token)> LoginAsync(LoginRequest request);
}