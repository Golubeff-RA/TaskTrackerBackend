using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourApp.DTOs.Auth;
using YourApp.Services.Interfaces;
using YourApp.Extensions;

namespace YourApp.Controllers
{
    /// <summary>
    /// Контроллер для аутентификации пользователей
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        /// <param name="registerDto">Данные для регистрации</param>
        /// <returns>Токены доступа</returns>
        /// <response code="200">Успешная регистрация</response>
        /// <response code="400">Ошибка валидации или пользователь уже существует</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponseDto), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                var result = await _authService.RegisterAsync(registerDto);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Вход пользователя
        /// </summary>
        /// <param name="loginDto">Данные для входа</param>
        /// <returns>Токены доступа</returns>
        /// <response code="200">Успешный вход</response>
        /// <response code="401">Неверные учетные данные</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDto), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var result = await _authService.LoginAsync(loginDto);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Обновление токена доступа
        /// </summary>
        /// <param name="refreshTokenDto">Refresh токен</param>
        /// <returns>Новая пара токенов</returns>
        /// <response code="200">Токен успешно обновлен</response>
        /// <response code="401">Недействительный refresh токен</response>
        [HttpPost("refresh")]
        [ProducesResponseType(typeof(AuthResponseDto), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto refreshTokenDto)
        {
            try
            {
                var result = await _authService.RefreshTokenAsync(refreshTokenDto.RefreshToken);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Выход пользователя (инвалидация refresh токена)
        /// </summary>
        /// <returns>Статус выхода</returns>
        /// <response code="200">Успешный выход</response>
        /// <response code="401">Требуется авторизация</response>
        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Logout()
        {
            var userId = User.GetUserId();
            await _authService.LogoutAsync(userId);
            return Ok(new { message = "Выход выполнен успешно" });
        }
    }
}