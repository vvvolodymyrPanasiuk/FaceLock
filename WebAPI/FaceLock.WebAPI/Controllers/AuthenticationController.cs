using FaceLock.Domain.Entities.UserAggregate;
using FaceLock.WebAPI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;

namespace FaceLock.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly FaceLock.Authentication.Services.IAuthenticationService _authenticationService;
        public AuthenticationController(
            UserManager<User> userManager, 
            ILogger<AuthenticationController> logger,
            FaceLock.Authentication.Services.IAuthenticationService authenticationService)
        {
            _userManager = userManager;
            _logger = logger;
            _authenticationService = authenticationService;
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
            if (ModelState.IsValid)
            {
                try
                {
                    bool result = await _authenticationService.RegisterAsync(new Authentication.DTO.UserRegisterDTO
                    {
                        Email = model.Email,
                        Password = model.Password,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                    });
                    // Check result
                    if (result)
                    {
                        // If user was created successfully, return status 200
                        return StatusCode(StatusCodes.Status201Created, $"Welcome, {model.FirstName} {model.LastName}!");
                    }

                    // If there was an error, return error message
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error creating user");
                }
                catch(Exception ex)
                {
                    // Log error and return 500 response
                    _logger.LogError($"Error: {ex.Message}");
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
                }
            }
            return BadRequest(ModelState);
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
                try
                {
                    (string accessToken, string refreshToken) = await _authenticationService.
                        LoginAsync(new Authentication.DTO.UserLoginDTO
                        {
                            Email = model.Email,
                            Password = model.Password
                        });

                    if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken)) 
                    {
                        _logger.LogError($"Authorization: Invalid tokens");
                        return StatusCode(StatusCodes.Status401Unauthorized, "Invalid tokens");
                    }

                    // Create cookie with HttpOnly for refreshToken 
                    Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions 
                    { 
                        HttpOnly = true, 
                        Secure = true
                    });

                    return StatusCode(StatusCodes.Status200OK, $"accessToken - {accessToken}, refreshToken - {refreshToken}");
                }
                catch (Exception ex)
                {
                    // Log error and return 500 response
                    _logger.LogError($"Error: {ex.Message}");
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
                }
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
                string accessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                //string refreshToken = await HttpContext.GetTokenAsync("refreshToken");
                string refreshToken = HttpContext.Request.Headers["refreshToken"].ToString();

                if (string.IsNullOrEmpty(refreshToken))
                {
                    return BadRequest();
                }

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return BadRequest();
                }
                // Revoke the refresh token
                bool result = await _authenticationService.LogoutAsync(refreshToken);
                if(result)
                {
                    return StatusCode(StatusCodes.Status204NoContent, "logged out successfully!");
                }

                // Revoke the refresh token
                //await _userManager.RemoveAuthenticationTokenAsync(user, JwtBearerDefaults.AuthenticationScheme, "JwtTokenSettings");
                //await _signInManager.SignOutAsync();

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during logout.");
            }
            catch (Exception ex)
            {
                // Log error and return 500 response
                _logger.LogError($"Error during logout: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during logout.");
            }
        }  
    }
}
