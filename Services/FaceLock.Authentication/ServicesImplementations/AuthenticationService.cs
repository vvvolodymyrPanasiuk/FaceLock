using FaceLock.Authentication.DTO;
using FaceLock.Authentication.Services;
using FaceLock.Domain.Entities.UserAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Authentication;

namespace FaceLock.Authentication.ServicesImplementations
{
    /// <summary>
    /// Authentication service implementation that implements IAuthenticationService interface.
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly ITokenService _tokenService;

        public AuthenticationService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<AuthenticationService> logger,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _tokenService = tokenService;
        }


        public async Task<(string, string)> LoginAsync(UserLoginDTO userLoginDto, UserMetaDataDTO userMetaDataDto)
        {
            // Find user by email
            var user = await _userManager.FindByEmailAsync(userLoginDto.Email);
            // If user not found, return authorization error
            if (user == null)
            {
                _logger.LogWarning($"User with login {userLoginDto.Email} not found");
                throw new AuthenticationException("Invalid login or password");
            }
            // Check password
            var result = await _signInManager.CheckPasswordSignInAsync(user, userLoginDto.Password, false);
            if (!result.Succeeded)
            {
                _logger.LogWarning($"Failed to authenticate user with login {userLoginDto.Email}");
                throw new AuthenticationException("Invalid username or password");
            }
            // Generate RefreshToken for user
            var refreshToken = await _tokenService.
                GenerateRefreshToken(user.Id, userMetaDataDto);
            // Generate AccessToken for user
            var accessToken = await _tokenService.GenerateAccessToken(user);
            
            return (accessToken, refreshToken);
        }

        public async Task<bool> RegisterAsync(UserRegisterDTO userRegisterDto)
        {
            // Check if user with same email already exists
            var existingUser = await _userManager.FindByEmailAsync(userRegisterDto.Email);
            if (existingUser != null)
            {
                _logger.LogError($"Failed to register user: A user with this login already exists.");
                throw new AuthenticationException("A user with this email already exists.");
            }
            // Create user with provided data
            var user = new User
            {
                UserName = userRegisterDto.Email,
                Email = userRegisterDto.Email,
                FirstName = userRegisterDto.FirstName,
                LastName = userRegisterDto.LastName,
                Status = "User"
            };
            // Save user to database
            var result = await _userManager.CreateAsync(user, userRegisterDto.Password);
            if (!result.Succeeded)
            {
                var errorMessage = string.Join("; ", result.Errors.Select(e => e.Description));
                _logger.LogError($"Failed to register user {user.UserName}: {errorMessage}");
                throw new AuthenticationException(errorMessage);
            }
            return result.Succeeded;
        }

        public async Task<bool> LogoutAsync(string refreshToken)
        {
            return await _tokenService.RevokeRefreshToken(refreshToken);
        }

        public async Task<string> RefreshAccessTokenAsync(string refreshToken)
        {
            return await _tokenService.RefreshAccessToken(refreshToken);
        }
    }
}
