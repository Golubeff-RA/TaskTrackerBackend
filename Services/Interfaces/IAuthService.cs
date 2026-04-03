// Services/Interfaces/IAuthService.cs
using System.Threading.Tasks;
using YourApp.DTOs.Auth;

namespace YourApp.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
        Task<bool> LogoutAsync(Guid userId);
        Task<bool> ValidateRefreshTokenAsync(string refreshToken, Guid userId);
    }
}