using FaceLock.Domain.Entities.UserAggregate;
using FaceLock.Domain.Repositories;
using FaceLock.Domain.Repositories.UserRepository;
using FaceLock.WebAPI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    [Authorize(Roles = "admin")]
    public class AdminUserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserFaceRepository _userFaceRepository;
        private readonly UserManager<User> _userManager;
        public AdminUserController(
            UserManager<User> userManager, 
            IUserRepository userRepository,
            IUserFaceRepository userFaceRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _userFaceRepository = userFaceRepository;
        }


        // POST api/<AdminUserController>/CreateUser
        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="model">UserViewModel with required fields</param>
        /// <returns>Status code 200 if successful, BadRequest if ModelState invalid, Conflict if user already exists</returns>
        [HttpPost("CreateUser")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateUser([FromBody] UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                /*
                // Check if required fields are present
                if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
                {
                    return BadRequest("Username and password are required");
                }
                */

                // Check if the username is available
                var existingUser = await _userRepository.GetUserByUsernameAsync(model.Username);
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

                var result = await _userManager.CreateAsync(user);
                //var result = await _userRepository.AddAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                // Assign the "user" role to the new user
                result = await _userManager.AddToRoleAsync(user, "user");
                if (result.Succeeded)
                {
                    return Ok();
                }

                // If adding the role failed, add errors to ModelState
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }


            // If ModelState is invalid, return BadRequest with ModelState errors
            return BadRequest(ModelState);
        }


        // GET api/<AdminUserController>/GetUsers
        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns>Status code 200 with a list of UserViewModels if successful, BadRequest otherwise</returns>
        [HttpGet("GetUsers")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUsers()
        {
            // Retrieve the users from the repository
            var users = await _userRepository.GetAllAsync();

            // Map the user entity to a UserViewModel object
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
            return Ok(result);
        }


        // GET api/<AdminUserController>/GetUser/{id}
        /// <summary>
        /// Gets a user by their ID
        /// </summary>
        /// <param name="id">The user's ID</param>
        /// <returns>Status code 200 with a UserViewModel if successful, NotFound otherwise</returns>
        [HttpGet("GetUser/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUser(string id)
        {
            // Retrieve the user with the specified ID from the repository
            var user = await _userRepository.GetByIdAsync(id);

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
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the user with the specified ID from the repository
                var user = await _userRepository.GetByIdAsync(id);
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
                var result = await _userManager.UpdateAsync(user);
                //await _userRepository.UpdateAsync(user);

                /*
                // Remove old roles first
                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, roles);

                // Add new role
                await _userManager.AddToRoleAsync(user, userDto.Role);
                */

                if (result.Succeeded)
                {
                    // Return an OK response if the update was successful
                    return Ok();
                }
                else
                {
                    // Return a BadRequest response if there were errors during the update
                    return BadRequest(result.Errors);
                }
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
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Delete the user.
            //await _userRepository.DeleteAsync(user);
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                //return NoContent();
                return Ok();
            }
            else
            {
                // Return any errors that occurred during deletion.
                return BadRequest(result.Errors);
            }
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
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddUserPhoto(string id, [FromForm] IFormFileCollection file)
        {
            if (ModelState.IsValid)
            {
                // Check if a user with the given ID exists
                var user = await _userRepository.GetByIdAsync(id);
            
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
                        await _userFaceRepository.AddAsync(userFace);
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
        public async Task<IActionResult> DeleteUserPhoto(string id, int faceId)
        {
            // Get the user with the given id
            var user = await _userRepository.GetByIdAsync(id);

            // If the user doesn't exist, return NotFound
            if (user == null)
            {
                return NotFound();
            }

            // Get all of the user's faces
            var userFaces = await _userFaceRepository.GetAllUserFacesAsync(id);

            // Find the face to delete
            var userFaceToDelete = userFaces.Find(f => f.Id == faceId);

            // If the face doesn't exist, return NotFound
            if (userFaceToDelete == null)
            {
                return NotFound();
            }

            // Delete the face
            await _userFaceRepository.DeleteAsync(userFaceToDelete);

            // Return Ok
            return Ok();
        }
    }
}
