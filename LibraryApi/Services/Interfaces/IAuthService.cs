using LibraryApi.Data.Auth;

namespace LibraryApi.Services.Interfaces;

public interface IAuthService
{
    Task<bool> RegisterAsync(ApiUserDto dto);
    Task<string?> LoginAsync(string username, string password);
    string WriteToken(User user);
}