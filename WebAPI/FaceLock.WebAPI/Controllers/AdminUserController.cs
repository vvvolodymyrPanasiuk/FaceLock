using FaceLock.DataManagement.Services;
using FaceLock.Domain.Entities.UserAggregate;
using FaceLock.WebAPI.Models.AdminUserModels.Request;
using FaceLock.WebAPI.Models.AdminUserModels.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FaceLock.WebAPI.Controllers
{
    /// <summary>
    /// Admin user API controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminUserController : ControllerBase
    {
        private readonly IDataServiceFactory _dataServiceFactory;
        private readonly ILogger<AdminUserController> _logger;
        private readonly Recognition.Services.IFaceRecognitionService<string> _recognitionService;

        public AdminUserController(
            IDataServiceFactory dataServiceFactory,
            ILogger<AdminUserController> logger,
            Recognition.Services.IFaceRecognitionService<string> recognitionService)
        {
            _dataServiceFactory = dataServiceFactory;
            _logger = logger;
            _recognitionService = recognitionService;
        }

        
        #region POST Metods

        // POST: api/<AuthenticationController>/CreateUser
        /// <summary>
        /// Creates a new user with the given details.
        /// </summary>
        /// <param name="model">The details of the user to be created.</param>
        /// <returns>Returns status 201 (Created) if the user was created successfully or an error message.</returns>
        /// <response code="201">Returns status 201 (Created) if the user was created successfully.</response>
        /// <response code="400">If the model state is not valid.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="409">If a user with the same email already exists.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpPost("CreateUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var query = _dataServiceFactory.CreateQueryUserService();
                    if (await query.IsExistUserByEmailAsync(model.Email) == true)
                    {
                        return StatusCode(StatusCodes.Status409Conflict, "User already exists.");
                    }

                    var command = _dataServiceFactory.CreateCommandUserService();
                    await command.AddUserAsync(new User
                    {
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserName = $"{model.FirstName}.{model.LastName}",
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
        /// <returns>Returns status 201 (Created) if the files were added successfully or an error message.</returns>
        /// <response code="201">Returns status 201 (Created) if the files were added successfully.</response>
        /// <response code="400">If the model state is not valid.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpPost("{userId}/AddUserPhotos")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUserPhotos(string userId, [FromForm] AddUserPhotosRequest files)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var face in files.Files)
                    {
                        await _recognitionService.AddUserFaceToTrainModelAsync(userId, face);
                    }

                    var command = _dataServiceFactory.CreateCommandUserService();
                    await command.AddUserFacesAsync(userId, files.Files);

                    return StatusCode(StatusCodes.Status201Created);
                }
                catch (ArgumentNullException ex)
                {
                    // Log error and return 400 response
                    _logger.LogError($"Error: {ex.Message}");
                    return StatusCode(StatusCodes.Status400BadRequest, $"Error: {ex.Message}");
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
        /// <returns>Returns status 201 (Created) if the photo was added successfully or an error message.</returns>
        /// <response code="201">Returns status 201 (Created) if the photo was added successfully.</response>
        /// <response code="400">If the model state is not valid.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="500">If an error occurred during the operation.</response>        
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpPost("{userId}/AddUserPhoto")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUserPhoto(string userId, [FromForm] AddUserPhotoRequest file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _recognitionService.AddUserFaceToTrainModelAsync(userId, file.File);

                    var command = _dataServiceFactory.CreateCommandUserService();
                    await command.AddUserFaceAsync(userId, file.File);

                    return StatusCode(StatusCodes.Status201Created);
                }
                catch (ArgumentNullException ex)
                {
                    // Log error and return 400 response
                    _logger.LogError($"Error: {ex.Message}");
                    return StatusCode(StatusCodes.Status400BadRequest, $"Error: {ex.Message}");
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
        /// <returns>Returns status 200 (OK) and the list of users or an error message.</returns>
        /// <response code="200">Returns status 200 (OK) and the list of users.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="500">If an error occurred during the operation.</response>        
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetUsersResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpGet("GetUsers")]
        [Authorize(Roles = "Admin")]
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
        /// <returns>Returns the user details if found or an error message.</returns>
        /// <response code="200">Returns the user details if found.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="404">If the user with the given ID was not found.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpGet("GetUser/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUser(string userId)
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryUserService();
                var user = await query.GetUserByIdAsync(userId);

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
        /// <returns>Returns the user's photo as a file stream or status 404 (Not Found) if the photo is not found.</returns>
        /// <response code="200">Returns the user's photo as a file stream.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="404">If the photo is not found.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpGet("{userId}/GetUserPhoto/{faceId}")]
        [Authorize(Roles = "Admin")]
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
        /// <returns>Returns the photos of the user in a compressed zip file.</returns>
        /// <response code="200">Returns the photos of the user in a compressed zip file.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="404">If a user with the given ID does not exist.</response>
        /// <response code="500">If an error occurred during the operation.</response>        
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpGet("{userId}/GetUserPhotos")]
        [Authorize(Roles = "Admin")]
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
        /// <returns>Returns status 200 (OK) with the list of user photos information or an error message.</returns>
        /// <response code="200">Returns status 200 (OK) with the list of user photos information.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not authorized to perform this action.</response>
        /// <response code="404">If the specified user does not exist.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(typeof(List<GetUserPhotosInfoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpGet("{userId}/GetUserPhotosInfo")]
        [Authorize(Roles = "Admin")]
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
        /// <returns>Returns status 201 (Created) if the user was updated successfully or an error message.</returns>
        /// <response code="201">Returns status 201 (Created) if the user was updated successfully.</response>
        /// <response code="400">If the model state is not valid.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="404">If the user with the specified ID was not found.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpPut("{userId}/UpdateUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUserRequest model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var query = _dataServiceFactory.CreateQueryUserService();
                    var user = await query.GetUserByIdAsync(userId);

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
        /// <returns>Returns status 204 (No Content) if the user was deleted successfully or an error message.</returns>
        /// <response code="204">Returns status 204 (No Content) if the user was deleted successfully.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="404">If a user with the specified ID was not found.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpDelete("{userId}/DeleteUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryUserService();
                var user = await query.GetUserByIdAsync(userId);

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
        /// <returns>Returns status 204 (No Content) if the users were deleted successfully or an error message.</returns>
        /// <response code="204">Returns status 204 (No Content) if the users were deleted successfully.</response>
        /// <response code="400">If the request body is invalid or the provided IDs are empty.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpDelete("deleteUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUsers([FromBody] DeleteUsersRequest usersId)
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryUserService();
                var usersForDelete = await query.GetUsersByIdAsync(usersId.UsersId);

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
        /// <returns>Returns status 204 (No Content) if the photo was deleted successfully or an error message.</returns>
        /// <response code="204">Returns status 204 (No Content) if the photo was deleted successfully.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="404">If the user or the user photo does not exist.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpDelete("{userId}/DeleteUserPhoto/{faceId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserPhoto(string userId, int faceId)
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryUserService();
                var userFaceToDelete = await query.GetUserFaceByIdAsync(userId, faceId);

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
        /// <returns>Returns status 204 (No Content) if the photos were deleted successfully or an error message.</returns>
        /// <response code="204">Returns status 204 (No Content) if the photos were deleted successfully.</response>
        /// <response code="400">If the model state is not valid.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="404">If the user or the specified photos do not exist.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpDelete("{userId}/DeleteUserPhotos")]
        [Authorize(Roles = "Admin")]
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
