using FaceLock.DataManagement.Services;
using FaceLock.Domain.Entities.UserAggregate;
using FaceLock.Domain.Repositories.UserRepository;
using FaceLock.EF.Repositories.UserRepository;
using FaceLock.WebAPI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace FaceLock.WebAPI.Controllers
{
    /// <summary>
    /// Controller for managing users with admin roles
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminUserController : ControllerBase
    {
        private readonly IDataServiceFactory _dataServiceFactory;
        private readonly ILogger<AdminUserController> _logger;

        public AdminUserController(
            IDataServiceFactory dataServiceFactory,
            ILogger<AdminUserController> logger)
        {
            _dataServiceFactory = dataServiceFactory;
            _logger = logger;
        }


        // POST api/<AdminUserController>/CreateUser
        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="model">UserViewModel with required fields</param>
        /// <returns>Status code 200 if successful, BadRequest if ModelState invalid, Conflict if user already exists</returns>
        [HttpPost("CreateUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if the username is available
                    var query = _dataServiceFactory.CreateQueryUserService();
                    var existingUser = query.GetUserByUsernameAsync(model.Username);
                    if (existingUser != null)
                    {
                        return Conflict("User already exists.");
                    }

                    // Create the new user
                    var user = new User
                    {
                        UserName = model.Username,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Status = model.Status
                    };

                    var command = _dataServiceFactory.CreateCommandUserService();
                    await command.AddUserAsync(user);

                    return StatusCode(StatusCodes.Status201Created);
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


        // GET api/<AdminUserController>/GetUsers
        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns>Status code 200 with a list of UserViewModels if successful, BadRequest otherwise</returns>
        [HttpGet("GetUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers()
        {
            var query = _dataServiceFactory.CreateQueryUserService();
            var users = await query.GetAllUsersAsync();

            var result = users.Select(u => new UserViewModel
            {
                Id = u.Id,
                //UserName = u.UserName,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Status = u.Status
            });

            // Return the list of UserViewModel object with a status code of 200
            return Ok(users);
        }


        // GET api/<AdminUserController>/GetUser/{id}
        /// <summary>
        /// Gets a user by their ID
        /// </summary>
        /// <param name="id">The user's ID</param>
        /// <returns>Status code 200 with a UserViewModel if successful, NotFound otherwise</returns>
        [HttpGet("GetUser/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUser(string id)
        {
            // Retrieve the user with the specified ID from the repository
            var query = _dataServiceFactory.CreateQueryUserService();
            var user = await query.GetUserByIdAsync(id);

            // Check if the user was found in the repository
            if (user == null)
            {
                // If the user was not found, return a NotFound status code
                return NotFound();
            }
            
            // TODO: добавити фото користувачів в резалт що повертає метод

            // Map the user entity to a UserViewModel object
            var result = new UserViewModel
            {
                Id = user.Id,
                //UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Status = user.Status
            };

            // Return the UserViewModel object with a status code of 200
            return Ok(result);
        }


        // PUT api/<AdminUserController>/UpdateUser/{id}
        /// <summary>
        /// Updates a user by id
        /// </summary>
        /// <param name="id">The id of the user to update.</param>
        /// <param name="model">The UserViewModel containing the updated user information.</param>
        /// <returns>Returns an IActionResult object indicating the success or failure of the operation.</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the user with the specified ID from the repository
                var query = _dataServiceFactory.CreateQueryUserService();
                var user = await query.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                // Update the user object with the data from the UserViewModel object
                //user.UserName = model.UserName ?? user.UserName;
                user.Email = model.Email ?? user.Email;
                user.FirstName = model.FirstName ?? user.FirstName;
                user.LastName = model.LastName ?? user.LastName;
                user.Status = model.Status ?? user.Status;

                // Update the user in the database using the UserManager
                var command = _dataServiceFactory.CreateCommandUserService();
                await command.UpdateUserAsync(user);
                //var result = await _userManager.UpdateAsync(user);
                //await _userRepository.UpdateAsync(user);

                return Ok();
                /*
                // Remove old roles first
                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, roles);

                // Add new role
                await _userManager.AddToRoleAsync(user, userDto.Role);
                */    
            }

            // Return a BadRequest response if the ModelState is invalid
            return BadRequest(ModelState);   
        }


        // DELETE api/<AdminUserController>/DeleteUser/{id}
        /// <summary>
        /// Deletes a user by id
        /// </summary>
        /// <param name="id">The id of the user to delete.</param>
        /// <returns>An IActionResult indicating whether the user was successfully deleted or an error message.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var query = _dataServiceFactory.CreateQueryUserService();
            var user = await query.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Delete the user.
            //await _userRepository.DeleteAsync(user);
            var command = _dataServiceFactory.CreateCommandUserService();
            await command.DeleteUserAsync(user);
            
            return Ok();

        }


        // POST api/<AdminUserController>/AddUserPhoto/{id}
        /// <summary>
        /// Adds a photo to the specified user's collection of faces.
        /// </summary>
        /// <param name="id">The id of the user to which to add the photo.</param>
        /// <param name="file">The file containing the photo to add.</param>
        /// <returns>Returns an IActionResult with HTTP status code 200 (OK) and the added user face entity if the operation is successful,
        /// or an IActionResult with HTTP status code 400 (Bad Request) and an error message if the operation fails.</returns>
        [HttpPost("{id}/photo")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUserPhoto(string id, [FromForm] IFormFileCollection file)
        {
            if (ModelState.IsValid)
            {
                // Check if a user with the given ID exists
                var query = _dataServiceFactory.CreateQueryUserService();
                var user = await query.GetUserByIdAsync(id);
            
                if (user == null)
                {
                    return NotFound("User not found");
                }
            
                foreach (var item in file)
                {
                    // Check the file type. We're only allowing image files, so uncomment the following block if you want to validate that.
                    /*
                    if (!file.ContentType.StartsWith("image/"))
                    {
                        return BadRequest("Invalid photo format");
                    }
                    */

                    // Check if a file was sent
                    if (item == null || item.Length == 0)
                    {
                        return BadRequest("No file was sent");
                    }

                    // Save the user's photo
                    using (var memoryStream = new MemoryStream())
                    {
                        await item.CopyToAsync(memoryStream);
                        // Upload the file if less than 2 MB  
                        //if (memoryStream.Length < 2097152) {}

                        var userFace = new UserFace
                        {
                            ImageData = memoryStream.ToArray(),
                            ImageMimeType = item.ContentType,
                            UserId = user.Id
                        };
                        var command = _dataServiceFactory.CreateCommandUserService();
                        await command.AddUserFaceAsync(userFace);
                        return Ok(userFace);
                    }
                }    
            }

            return BadRequest(ModelState);
        }


        // DELETE api/<AdminUserController>/{id}/DeleteUserPhoto/{id}
        /// <summary>
        /// Deletes a user's photo with the given faceId
        /// </summary>
        /// <param name="id">The id of the user</param>
        /// <param name="faceId">The id of the user's photo to delete</param>
        /// <returns>An IActionResult indicating success or failure</returns>
        [HttpDelete("{id}/DeleteUserPhoto/{faceId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserPhoto(string id, int faceId)
        {
            // Get the user with the given id
            var query = _dataServiceFactory.CreateQueryUserService();
            var user = query.GetUserByIdAsync(id);

            // If the user doesn't exist, return NotFound
            if (user == null)
            {
                return NotFound();
            }

            // Get all of the user's faces
            var userFaces = await query.GetAllUserFacesAsync(user.Result.Id);

            // Find the face to delete
            var userFaceToDelete = userFaces.FirstOrDefault(f => f.Id == faceId);

            // If the face doesn't exist, return NotFound
            if (userFaceToDelete == null)
            {
                return NotFound();
            }

            // Delete the face
            var command = _dataServiceFactory.CreateCommandUserService();
            await command.DeleteUserFaceAsync(userFaceToDelete);

            // Return Ok
            return Ok();
        }
    }
}
