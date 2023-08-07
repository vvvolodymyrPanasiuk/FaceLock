using FaceLock.Domain.Entities.UserAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Authentication;
using FaceLock.WebAPI.Models.AuthenticationModels.Request;
using FaceLock.WebAPI.Models.AuthenticationModels.Response;

namespace FaceLock.WebAPI.Controllers
{
    /// <summary>
    /// Authentication API controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly Authentication.Services.IAuthenticationService _authenticationService;
        public AuthenticationController(
            UserManager<User> userManager, 
            ILogger<AuthenticationController> logger,
            Authentication.Services.IAuthenticationService authenticationService)
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
        /// <returns>Returns status 201 (Created) if the user was registered successfully or an error message.</returns>
        /// <response code="201">Returns status 201 (Created) if the user was registered successfully.</response>
        /// <response code="400">If the model state is not valid or there was an authentication exception.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
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
        /// <returns>Returns status 200 (OK) with access and refresh tokens if authentication was successful or an error message.</returns>
        /// <response code="200">Returns status 200 (OK) with access and refresh tokens if authentication was successful.</response>
        /// <response code="400">If the model state is not valid or the user metadata is invalid.</response>
        /// <response code="500">If an error occurred during the operation or the tokens are invalid.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
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
        /// <remarks>
        /// This endpoint logs out the user by revoking the refresh token and clearing cookies.
        /// The user must be authenticated to use this endpoint.
        /// </remarks>
        /// <returns>Returns status 204 (No Content) if the user was logged out successfully, or an error message.</returns>
        /// <response code="204">Returns status 204 (No Content) if the user was logged out successfully.</response>
        /// <response code="400">If the authentication token is missing or invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
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
        /// <remarks>
        /// Requires an authorized user to perform this action. Returns a new access token if the given refresh token is valid and not expired.
        /// </remarks>
        /// <returns>Returns status 200 (OK) with a new access token if the given refresh token is valid and not expired or an error message.</returns>
        /// <response code="200">Returns status 200 (OK) with a new access token if the given refresh token is valid and not expired.</response>
        /// <response code="400">If the authorization header is missing or the refresh token is invalid or expired.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RefreshResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
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
