using FaceLock.Domain.Entities.UserAggregate;
using FaceLock.WebAPI.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace FaceLock.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(
            SignInManager<User> signInManager, 
            UserManager<User> userManager, 
            IConfiguration configuration,
            ILogger<AuthenticationController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }

        // POST: api/<AuthenticationController>/register
        /// <summary>
        /// User registration
        /// </summary>
        /// <param name="model">User data to be registered</param>
        /// <returns>Status 200 or error message</returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // Check if user data is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if user with same email already exists
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError(nameof(model.Email), "A user with this email already exists.");
                return BadRequest(ModelState);
            }

            // Create user with provided data
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Status = "user"
            };

            // Save user to database
            var result = await _userManager.CreateAsync(user, model.Password);

            // Check result
            if (result.Succeeded)
            {
                // If user was created successfully, return status 200
                return Ok(new { Message = $"Welcome, {user.FirstName} {user.LastName}!" });
            }

            // Log errors
            foreach (var error in result.Errors)
            {
                _logger.LogError($"Error creating user: {error.Code} - {error.Description}");
            }

            // If there was an error, return error message
            return StatusCode(500, "Error creating user");
        }

        // POST: api/<AuthenticationController>/login
        /// <summary>
        /// Login a user
        /// </summary>
        /// <param name="model">User credentials required for login</param>
        /// <returns>Status 200 with token or error message</returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find user by email
                var user = await _userManager.FindByEmailAsync(model.Email);

                // If user not found, return authorization error
                if (user == null)
                {
                    _logger.LogError($"Authorization: Invalid login credentials for user {model.Email}");
                    return Unauthorized(new { message = "Invalid login credentials" });
                }

                // Check password
                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                // If password is incorrect, return authorization error
                if (!result.Succeeded)
                {
                    _logger.LogError($"Authorization: Invalid login credentials for user {model.Email}");
                    return Unauthorized(new { message = "Invalid login credentials" });
                }
                // Generate JWT token for user
                var token = GenerateJwtToken(user);
                await _userManager.SetAuthenticationTokenAsync(user, JwtBearerDefaults.AuthenticationScheme, "JwtTokenSettings", token.ToString());

                // Save token in Authorization header
                //HttpContext.Response.Headers.Add("Authorization", "Bearer " + token.ToString());

                return Ok(new { Token = token.ToString() });
            }

            return BadRequest(ModelState);
        }

        // POST: api/<AuthenticationController>/logout
        /// <summary>
        /// Logs out the user and clears cookies.
        /// </summary>
        /// <param></param>
        /// <returns>Returns status 200 or an error message.</returns>
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return BadRequest();
                }

                // Revoke the refresh token
                await _userManager.RemoveAuthenticationTokenAsync(user, JwtBearerDefaults.AuthenticationScheme, "JwtTokenSettings");
                
                // Clear the authentication cookies
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignOutAsync(JwtBearerDefaults.AuthenticationScheme);

                await _signInManager.SignOutAsync();

                return Ok(new { Message = $"{user.FirstName} {user.LastName} logged out successfully!" });
            }     
            catch(Exception ex)
            {
                // Log error and return 500 response
                _logger.LogError($"Error during logout: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during logout.");
            }
        }

        // Generates a JWT token for an authorized user
        private async Task<string> GenerateJwtToken(User user)
        {
            try
            {
                // Define claims for the JWT token
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.GivenName, user.FirstName),
                    new Claim(ClaimTypes.Surname, user.LastName),
                    new Claim(ClaimTypes.Role , user.Status)
                };

                // Use a symmetric key to sign the JWT token
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecretKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                // Define the lifetime of the JWT token
                var expires = DateTime.UtcNow.AddDays(int.Parse(_configuration.GetValue<string>("JwtTokenSettings:RefreshExpirationDays")));

                // Create a JWT token with the defined parameters
                var token = new JwtSecurityToken(
                    issuer: _configuration.GetValue<string>("JwtTokenSettings:Issuer"),
                    audience: _configuration.GetValue<string>("JwtTokenSettings:Audience"),
                    claims: claims,
                    expires: expires,
                    signingCredentials: creds
                );

                await _userManager.RemoveAuthenticationTokenAsync(user, JwtBearerDefaults.AuthenticationScheme, "JwtTokenSettings");

                return await Task.Run(() => new JwtSecurityTokenHandler().WriteToken(token));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating JWT token");
                throw new ApplicationException("Error generating JWT token");
            }
        }
    }
}
