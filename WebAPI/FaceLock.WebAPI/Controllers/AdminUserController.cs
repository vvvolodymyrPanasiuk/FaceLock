using FaceLock.DataManagement.Services;
using FaceLock.Domain.Entities.UserAggregate;
using FaceLock.WebAPI.Models.AdminUserModels.Request;
using FaceLock.WebAPI.Models.AdminUserModels.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FaceLock.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
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


        #region POST Metods

        // POST: api/<AuthenticationController>/CreateUser
        /// <summary>
        /// Creates a new user with the given details.
        /// </summary>
        /// <param name="model">The details of the user to be created.</param>
        /// <returns>Returns status 201 if successful or an error message if not.</returns>
        [HttpPost("CreateUser")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var query = _dataServiceFactory.CreateQueryUserService();
                    var existingUser = query.GetUserByEmailAsync(model.Email);

                    if (existingUser.Result != null)
                    {
                        return StatusCode(StatusCodes.Status409Conflict, "User already exists.");
                    }

                    var command = _dataServiceFactory.CreateCommandUserService();
                    await command.AddUserAsync(new User
                    {
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Status = model.Status
                    });

                    return StatusCode(StatusCodes.Status201Created);
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

        // POST api/<AdminUserController>/{userId}/AddUserPhotos
        /// <summary>
        /// Adds one or more photos to the user's account.
        /// </summary>
        /// <param name="userId">The ID of the user whose account the photos are being added to.</param>
        /// <param name="files">The files to be uploaded as the user's photo(s).</param>
        /// <returns>Returns status 201 or an error message.</returns>
        [HttpPost("{userId}/AddUserPhotos")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUserPhotos(string userId, [FromForm] AddUserPhotosRequest files)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var command = _dataServiceFactory.CreateCommandUserService();
                    await command.AddUserFacesAsync(userId, files.Files);

                    return StatusCode(StatusCodes.Status201Created);
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

        // POST api/<AdminUserController>/{userId}/AddUserPhoto
        /// <summary>
        /// Adds a photo to a user's face by the user ID.
        /// </summary>
        /// <param name="userId">The user ID to add a photo to.</param>
        /// <param name="file">The file to upload as the user's face photo.</param>
        /// <returns>Returns status 201 if the photo was added successfully, or an error message otherwise.</returns>
        [HttpPost("{userId}/AddUserPhoto")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUserPhoto(string userId, [FromForm] AddUserPhotoRequest file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var command = _dataServiceFactory.CreateCommandUserService();
                    await command.AddUserFaceAsync(userId, file.File);

                    return StatusCode(StatusCodes.Status201Created);
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

        #endregion


        #region GET Metods

        // GET api/<AdminUserController>/GetUsers
        /// <summary>
        /// Retrieves a list of all users.
        /// </summary>
        /// <returns>Returns a list of GetUserResponse objects or an error message.</returns>
        [HttpGet("GetUsers")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryUserService();
                var users = await query.GetAllUsersAsync();

                var result = users.Select(u =>
                    new GetUserResponse(u.Id, u.UserName, u.Email, u.FirstName, u.LastName, u.Status));

                return StatusCode(StatusCodes.Status200OK, new GetUsersResponse(result));
            }
            catch (Exception ex)
            {
                // Log error and return 500 response
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }           
        }


        // GET api/<AdminUserController>/GetUser/{userId}
        /// <summary>
        /// Retrieves the user with the specified ID from the repository.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve.</param>
        /// <returns>Returns status 200 and the user data or 404 
        /// if the user is not found or 500 if an error occurred.</returns>
        [HttpGet("GetUser/{userId}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUser(string userId)
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryUserService();
                var user = await query.GetUserByIdAsync(userId);

                if (user == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                return StatusCode(StatusCodes.Status200OK,
                    new GetUserResponse(user.Id, user.UserName, user.Email,
                        user.FirstName, user.LastName, user.Status));
            }
            catch (Exception ex)
            {
                // Log error and return 500 response
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }          
        }


        // GET api/<AdminUserController>/{userId}/GetUserPhoto/{faceId}
        /// <summary>
        /// Retrieves a user's photo by ID and face ID.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <param name="faceId">Face ID.</param>
        /// <returns>Returns the user's photo or an error message.</returns>
        [HttpGet("{userId}/GetUserPhoto/{faceId}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserPhoto(string userId, int faceId)
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryUserService();
                var userFace = await query.GetUserFaceByIdAsync(userId, faceId);

                if (userFace?.ImageData != null)
                {
                    // return the image as a file stream
                    return File(userFace.ImageData, userFace.ImageMimeType);
                }

                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                // Log error and return 500 response
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }       
        }


        // GET api/<AdminUserController>/{userId}/GetUserPhotos
        /// <summary>
        /// Retrieves all the photos of the user with the given ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>Returns a zip archive of the user's photos as a file download.</returns>
        [HttpGet("{userId}/GetUserPhotos")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserPhotos(string userId)
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryUserService();
                var userFaces = await query.GetAllUserFacesAsync(userId);

                GetUserPhotosResponse result = new GetUserPhotosResponse(userId, userFaces);

                return File(result.FileContents, result.ContentType, result.FileDownloadName);
            }
            catch (Exception ex)
            {
                // Log error and return 500 response
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }


        // GET api/<AdminUserController>/{userId}/GetUserPhotosInfo
        /// <summary>
        /// Retrieves information about all photos belonging to a user with the specified ID.
        /// </summary>
        /// <param name="userId">The ID of the user whose photos to retrieve.</param>
        /// <returns>Returns status 200 and a list of GetUserPhotosInfoResponse objects, or an error message.</returns>
        [HttpGet("{userId}/GetUserPhotosInfo")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserPhotosInfo(string userId)
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryUserService();
                var userFaces = await query.GetAllUserFacesAsync(userId);

                var result = new List<GetUserPhotosInfoResponse>();

                foreach (var userFace in userFaces)
                {
                    result.Add(new GetUserPhotosInfoResponse(userFace.Id,
                        userFace.ImageMimeType, userFace.UserId));
                }

                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (Exception ex)
            {
                // Log error and return 500 response
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        #endregion


        #region PUT Metods

        // PUT api/<AdminUserController>/{userId}/UpdateUser
        /// <summary>
        /// Updates a user by ID.
        /// </summary>
        /// <param name="userId">The ID of the user to update.</param>
        /// <param name="model">The model containing the updated user data.</param>
        /// <returns>Returns status 201 if successful or an error message.</returns>
        [HttpPut("{userId}/UpdateUser")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUserRequest model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var query = _dataServiceFactory.CreateQueryUserService();
                    var user = await query.GetUserByIdAsync(userId);
                    if (user == null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound);
                    }

                    user.UserName = model.UserName ?? user.UserName;
                    user.Email = model.Email ?? user.Email;
                    user.FirstName = model.FirstName ?? user.FirstName;
                    user.LastName = model.LastName ?? user.LastName;
                    user.Status = model.Status ?? user.Status;

                    var command = _dataServiceFactory.CreateCommandUserService();
                    await command.UpdateUserAsync(user);

                    return StatusCode(StatusCodes.Status201Created);
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

        #endregion


        #region DELETE Metods

        // DELETE api/<AdminUserController>/{userId}/DeleteUser
        /// <summary>
        /// Deletes the user with the specified ID.
        /// </summary>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <returns>Returns status 204 if successful or an error message.</returns>
        [HttpDelete("{userId}/DeleteUser")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryUserService();
                var user = await query.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent);
                }

                var command = _dataServiceFactory.CreateCommandUserService();
                await command.DeleteUserAsync(user);

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                // Log error and return 500 response
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }


        // DELETE api/<AdminUserController>/DeleteUsers
        /// <summary>
        /// Deletes multiple users.
        /// </summary>
        /// <param name="usersId">The request containing the IDs of the users to be deleted.</param>
        /// <returns>Returns status 204 if the operation was successful or an error message otherwise.</returns>
        [HttpDelete("deleteUsers")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUsers([FromBody] DeleteUsersRequest usersId)
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryUserService();
                var usersForDelete = await query.GetUsersByIdAsync(usersId.UsersId);

                if (usersForDelete == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                var command = _dataServiceFactory.CreateCommandUserService();
                await command.DeleteUsersAsync(usersForDelete);

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                // Log error and return 500 response
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }


        // DELETE api/<AdminUserController>/{userId}/DeleteUserPhoto/{faceId}
        /// <summary>
        /// Deletes a user's face photo from the database by ID.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <param name="faceId">User's face ID.</param>
        /// <returns>Returns status 204 if deleted or 404 if not found. Returns 500 if an error occurred.</returns>
        [HttpDelete("{userId}/DeleteUserPhoto/{faceId}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserPhoto(string userId, int faceId)
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryUserService();
                var userFaceToDelete = await query.GetUserFaceByIdAsync(userId, faceId);

                if (userFaceToDelete == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                var command = _dataServiceFactory.CreateCommandUserService();
                await command.DeleteUserFaceAsync(userFaceToDelete);

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                // Log error and return 500 response
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }


        // DELETE api/<AdminUserController>/{userId}/DeleteUserPhotos
        /// <summary>
        /// Deletes the user's photo by id.
        /// </summary>
        /// <param name="userId">The id of the user whose photo needs to be deleted.</param>
        /// <param name="userFacesId">The id of the photo to delete.</param>
        /// <returns>Returns status 204 if successful or an error message.</returns>
        [HttpDelete("{userId}/DeleteUserPhotos")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserPhotos(string userId, [FromBody] DeleteUserPhotosRequest userFacesId)
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryUserService();
                var userFaceToDelete = await query.GetUserFacesByIdAsync(userId, userFacesId.userFacesId);

                var command = _dataServiceFactory.CreateCommandUserService();
                await command.DeleteUserFacesAsync(userFaceToDelete);

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                // Log error and return 500 response
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }

        }

        #endregion

    }
}
