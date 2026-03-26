using Microsoft.EntityFrameworkCore;
using AuthApi.Data;
using AuthApi.Models;
using AuthApi.Models.DTOs;

namespace AuthApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthService> _logger;
        
        public AuthService(
            ApplicationDbContext context, 
            IJwtService jwtService,
            ILogger<AuthService> logger)
        {
            _context = context;
            _jwtService = jwtService;
            _logger = logger;
        }
        
        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            // Проверяем, существует ли пользователь
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == registerDto.Email || u.Username == registerDto.Username);
                
            if (existingUser != null)
            {
                throw new InvalidOperationException("Пользователь с таким email или именем уже существует");
            }
            
            // Создаем нового пользователя
            var user = new User
            {
                Email = registerDto.Email,
                Username = registerDto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            // Генерируем токены
            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();
            
            // Сохраняем refresh token
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();
            
            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Username = user.Username,
                Email = user.Email,
                UserId = user.Id
            };
        }
        
        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            // Ищем пользователя
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.UsernameOrEmail || 
                                         u.Username == loginDto.UsernameOrEmail);
                                         
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Неверный логин или пароль");
            }
            
            // Генерируем новые токены
            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();
            
            // Обновляем refresh token
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            
            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Username = user.Username,
                Email = user.Email,
                UserId = user.Id
            };
        }
        
        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
                
            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Недействительный или истекший refresh token");
            }
            
            var newAccessToken = _jwtService.GenerateAccessToken(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken();
            
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();
            
            return new AuthResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                Username = user.Username,
                Email = user.Email,
                UserId = user.Id
            };
        }
        
        public async Task<bool> LogoutAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}