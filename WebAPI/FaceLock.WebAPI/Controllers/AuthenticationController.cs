using FaceLock.Domain.Entities.UserAggregate;
using FaceLock.WebAPI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Authentication;
using FaceLock.WebAPI.Models.AuthenticationModels.Request;
using FaceLock.WebAPI.Models.AuthenticationModels.Response;

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
        public async Task<IActionResult> Register(RegisterRequest model)
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
                        // If user was created successfully, return status 201
                        return StatusCode(StatusCodes.Status201Created, $"Welcome, {model.FirstName} {model.LastName}!");
                    }
                    
                    // If there was an error, return error message
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error creating user");
                }
                catch (AuthenticationException ex)
                {
                    // Handle authentication exception and return 400 response
                    _logger.LogWarning($"Authorization: {ex.Message}");
                    return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
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

        // POST: api/<AuthenticationController>/login
        /// <summary>
        /// Login a user
        /// </summary>
        /// <param name="model">User credentials required for login</param>
        /// <returns>Status 200 with token or error message</returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string country = HttpContext.Request.Headers["country"].ToString();
                    string city = HttpContext.Request.Headers["city"].ToString();
                    string device = HttpContext.Request.Headers["device"].ToString();

                    if (string.IsNullOrEmpty(country) || string.IsNullOrEmpty(city) || string.IsNullOrEmpty(device))
                    {
                        _logger.LogError($"Authorization: Invalid user metadata");
                        return StatusCode(StatusCodes.Status400BadRequest, "Invalid user metadata");
                    }

                    (string accessToken, string refreshToken) = await _authenticationService.
                        LoginAsync(new Authentication.DTO.UserLoginDTO
                        {
                            Email = model.Email,
                            Password = model.Password
                        }, new Authentication.DTO.UserMetaDataDTO 
                        {
                            Country = country,
                            City = city,
                            Device = device
                        });

                    if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken)) 
                    {
                        _logger.LogError($"Authorization: Invalid tokens");
                        return StatusCode(StatusCodes.Status500InternalServerError, "Invalid tokens");
                    }

                    return StatusCode(StatusCodes.Status200OK, new LoginResponse(refreshToken, accessToken));
                }
                catch (AuthenticationException ex)
                {
                    // Handle authentication exception and return 400 response
                    _logger.LogWarning($"Authorization: {ex.Message}");
                    return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
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
        /// <returns>Returns status 204 or an error message.</returns>
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                string refreshToken = HttpContext.Request.Headers["refreshToken"].ToString();
                if (string.IsNullOrEmpty(refreshToken))
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, "Refresh token is empty.");
                }

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, "User is not already exists");
                }
                // Revoke the refresh token
                bool result = await _authenticationService.LogoutAsync(refreshToken);
                if (result)
                {
                    return StatusCode(StatusCodes.Status204NoContent, "logged out successfully!");
                }

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during logout.");
            }
            catch (AuthenticationException ex)
            {
                // Handle authentication exception and return 400 response
                _logger.LogWarning($"Authorization: {ex.Message}");
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                // Log error and return 500 response
                _logger.LogError($"Error during logout: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during logout.");
            }
        }

        // POST: api/<AuthenticationController>/refresh
        /// <summary>
        /// Refresh accessToken by refreshToken.
        /// </summary>
        /// <param></param>
        /// <returns>Returns status 200 with accessToken or an error message.</returns>
        [HttpPost("refresh")]
        [Authorize]
        public async Task<IActionResult> Refresh()
        {
            try
            {
                string refreshToken = HttpContext.Request.Headers["refreshToken"].ToString();

                // Check exist refreshToken
                if (string.IsNullOrEmpty(refreshToken))
                {
                    _logger.LogError("Authorization: refreshToken is missing from HttpOnlyCookie");
                    return StatusCode(StatusCodes.Status401Unauthorized, "refreshToken is missing");
                }

                var accessToken = await _authenticationService.RefreshAccessTokenAsync(refreshToken);
                if (string.IsNullOrEmpty(accessToken))
                {
                    _logger.LogError($"Authorization: Invalid tokens");
                    return StatusCode(StatusCodes.Status500InternalServerError, "Invalid tokens");
                }

                return StatusCode(StatusCodes.Status200OK, new RefreshResponse(accessToken));
            }
            catch (AuthenticationException ex)
            {
                // Handle authentication exception and return 400 response
                _logger.LogWarning($"Authorization: {ex.Message}");
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                // Log error and return 500 response
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
    }
}
