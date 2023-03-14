using FaceLock.Domain.Entities.UserAggregate;
using FaceLock.WebAPI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FaceLock.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        // Конструктор AuthController містить інстанси SignInManager,
        // UserManager та IConfiguration, які потрібні для авторизації та реєстрації користувачів
        public AuthenticationController(
            SignInManager<User> signInManager, 
            UserManager<User> userManager, 
            IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }


        /// <summary>
        /// Реєстрація користувача
        /// </summary>
        /// <param name="model">Дані користувача, які необхідно зареєструвати</param>
        /// <returns>Статус 200 з токеном або повідомлення про помилку</returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                // Перевіряємо чи дані користувача валідні
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Створення користувача з введеними даними
                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                // Збереження користувача в базу даних
                var result = await _userManager.CreateAsync(user, model.Password);

                // Перевірка результату
                if (!result.Succeeded)
                {
                    // Якщо сталася помилка, повертаємо повідомлення про помилку
                    return BadRequest(result.Errors);
                }

                // Якщо користувач успішно створений, створюємо токен для нього
                var tokenString = await GenerateJwtToken(user);

                // Повертаємо статус 200 з токеном
                return Ok(new { token = tokenString, UserId = user.Id });
            }
            catch (Exception ex)
            {
                // Якщо сталася помилка, повертаємо повідомлення про помилку
                return BadRequest(ex.Message);
            }
        }

        // POST: api/account/login
        /// <summary>
        /// Логін користувача
        /// </summary>
        /// <param name="model">Дані користувача, які необхідно ввести для логіна</param>
        /// <returns>Статус 200 з токеном або повідомлення про помилку</returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Знайти користувача по email
                var user = await _userManager.FindByEmailAsync(model.Email);

                // Якщо користувач не знайдений, повертаємо помилку авторизації
                if (user == null)
                {
                    return Unauthorized(new { message = "Invalid login credentials" });
                }

                // Перевірка пароля
                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                // Якщо пароль не правильний, повертаємо помилку авторизації
                if (!result.Succeeded)
                {
                    return Unauthorized(new { message = "Invalid login credentials" });
                }

                // Генерація JWT токена для користувача
                var token = GenerateJwtToken(user);

                return Ok(new { Token = token, UserId = user.Id });
            }

            return BadRequest(ModelState);
        }

        // POST: api/account/logout
        /// <summary>
        /// Логаут користувача і очищення кукі
        /// </summary>
        /// <param></param>
        /// <returns>Статус 200 або повідомлення про помилку</returns>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Ok(new { Message = "Logged out successfully!" });

        }

        // Генерує JWT-токен для авторизованого користувача
        private async Task<string> GenerateJwtToken(User user)
        {
            // Визначаємо клейми для JWT-токену
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName)
            };

            // Використовуємо симетричний ключ для підпису JWT-токену
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // Визначаємо час життя JWT-токену
            //DateTime.UtcNow.AddDays(7)
            var expires = DateTime.UtcNow.AddDays(int.Parse(_configuration["JwtExpireDays"]));

            // Створюємо JWT-токен з визначеними параметрами
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtIssuer"],
                audience: _configuration["JwtIssuer"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
