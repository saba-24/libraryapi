using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibraryApi.Data.Auth;
using LibraryApi.Data.Db;
using LibraryApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace LibraryApi.Services.Implementations;

public class AuthService(IConfiguration configuration, AppDbContext context) : IAuthService
{
    public async Task<bool> RegisterAsync(ApiUserDto dto)
    {
        if (context.ApiUsers.Any(x => x.Username == dto.Username)) return false;
        var user = new User();
        var hashedpassword = new PasswordHasher<User>().HashPassword(user, dto.Password);
        user.Username = dto.Username;
        user.PasswordHash = hashedpassword;
        context.ApiUsers.Add(user);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<string?> LoginAsync(string username, string password)
    {
        if(!context.ApiUsers.Any(x => x.Username == username)) return null;
        User u =  context.ApiUsers.FirstOrDefault(x => x.Username == username);
        if (new PasswordHasher<User>().VerifyHashedPassword(u, u.PasswordHash, password) ==
            PasswordVerificationResult.Success)
        {
            return WriteToken(u);
        }
        return null;
    }

    public string WriteToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("userId", user.Id.ToString())
        };
        var token = new JwtSecurityToken(
            configuration["JwtSettings:Issuer"],
            configuration["JwtSettings:Audience"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(configuration["JwtSettings:ExpiresMinutes"])),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}