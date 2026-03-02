using UserAuthApi.Models;

namespace UserAuthApi.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}