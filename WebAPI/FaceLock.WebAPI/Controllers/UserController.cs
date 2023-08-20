using FaceLock.DataManagement.Services;
using FaceLock.WebAPI.Models.AdminUserModels.Response;
using FaceLock.WebAPI.Models.UserModels.Request;
using FaceLock.WebAPI.Models.UserModels.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace FaceLock.WebAPI.Controllers
{
    /// <summary>
    /// User API controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IDataServiceFactory _dataServiceFactory;
        private readonly ILogger<UserController> _logger;

        public UserController(
            IDataServiceFactory dataServiceFactory,
            ILogger<UserController> logger)
        {
            _dataServiceFactory = dataServiceFactory;
            _logger = logger;
        }


        #region POST Metods
        #endregion


        #region GET Metods

        // GET: api/<UserController>/GetUserInfo
        /// <summary>
        /// Gets user information for the authenticated user.
        /// </summary>
        /// <returns>Returns status 200 (OK) and the user information or an error message.</returns>
        /// <response code="200">Returns status 200 (OK) and the user information.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetUserInfoResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpGet("GetUserInfo")]
        [Authorize]
        public async Task<IActionResult> GetUserInfo()
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryUserService();
                var user = await query.GetUserByIdAsync(
                    User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

                return StatusCode(StatusCodes.Status200OK,
                    new GetUserInfoResponse(user.Id, user.UserName, user.Email,
                        user.FirstName, user.LastName, user.Status));
            }
            catch (Exception ex)
            {
                // Log error and return 500 response
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }


        // GET: api/<UserController>/GetUserVisits
        /// <summary>
        /// Gets a list of user's visits to different places.
        /// </summary>
        /// <param></param>
        /// <returns>Returns status 200 (OK) and the list of visits made by the user.</returns>
        /// <response code="200">Returns status 200 (OK) and the list of visits made by the user.</response>
        /// <response code="401">If the user is not authenticated to perform this action.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetUserVisitsResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpGet("GetUserVisits")]
        [Authorize]
        public async Task<IActionResult> GetUserVisits()
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryPlaceService();
                var visits = await query.GetVisitsByUserIdAsync(
                    User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
                
                var result = visits.Select(u =>
                    new UserVisitByUserResponse(u.Id, u.UserId, u.PlaceId, u.CheckInTime, u.CheckOutTime));

                return StatusCode(StatusCodes.Status200OK, new GetUserVisitsResponse(result));
            }
            catch (Exception ex)
            {
                // Log error and return 500 response
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }


        // GET: api/<UserController>/GetUserAccesses
        /// <summary>
        /// Gets the list of door lock accesses for the authenticated user.
        /// </summary>
        /// <param></param>
        /// <returns>Returns status 200 (OK) and a list of accesses if successful or an error message.</returns>
        /// <response code="200">Returns status 200 (OK) and a list of accesses if successful.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetUserAccessesResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpGet("GetUserAccesses")]
        [Authorize]
        public async Task<IActionResult> GetUserAccesses()
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryDoorLockService();
                var accesses = await query.GetAccessByUserIdAsync(
                    User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

                var result = accesses.Select(u =>
                    new UserAccess(u.Id, u.UserId, u.DoorLockId, u.HasAccess));

                return StatusCode(StatusCodes.Status200OK, new GetUserAccessesResponse(result));
            }
            catch (Exception ex)
            {
                // Log error and return 500 response
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }


        // GET: api/<UserController>/GetUserHistories
        /// <summary>
        /// Retrieves the user's door lock histories.
        /// </summary>
        /// <remarks>The user must be authenticated to access this endpoint.</remarks>
        /// <returns>Returns status 200 (OK) and a list of door lock usage history or an error message.</returns>
        /// <response code="200">Returns status 200 (OK) and a list of door lock usage history.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetUserHistoriesResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpGet("GetUserHistories")]
        [Authorize]
        public async Task<IActionResult> GetUserHistories()
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryDoorLockService();
                var history = await query.GetDoorLockHistoryByUserIdAsync(
                    User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

                var result = history.Select(u =>
                    new UserHistory(u.Id, u.UserId, u.DoorLockId, u.OpenedDateTime));

                return StatusCode(StatusCodes.Status200OK, new GetUserHistoriesResponse(result));
            }
            catch (Exception ex)
            {
                // Log error and return 500 response
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }


        // GET api/<UserController>/GetUserPhoto/{faceId}
        /// <summary>
        /// Retrieves a user's photo by ID and face ID.
        /// </summary>
        /// <param name="faceId">Face ID.</param>
        /// <returns>Returns the user's photo as a file stream or status 404 (Not Found) if the photo is not found.</returns>
        /// <response code="200">Returns the user's photo as a file stream.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="404">If the photo is not found.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(int))]
        [HttpGet("GetUserPhoto/{faceId}")]
        [Authorize]
        public async Task<IActionResult> GetUserPhoto(int faceId)
        {
            try
            {
                //User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
                var query = _dataServiceFactory.CreateQueryUserService();
                var userFace = await query.GetUserFaceByIdAsync(
                    User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, faceId);

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


        // GET api/<UserController>/GetUserPhotosInfo
        /// <summary>
        /// Retrieves all the photos of the user with the given ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>Returns the photos of the user.</returns>
        /// <response code="200">Returns the photos of the user in a compressed zip file.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="404">If a user with the given ID does not exist.</response>
        /// <response code="500">If an error occurred during the operation.</response>        
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("GetUserPhotosInfo")]
        [Authorize]
        public async Task<IActionResult> GetUserPhotosInfo()
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryUserService();
                var userFaces = await query.GetAllUserFacesAsync(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

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

        // PUT api/<AdminUserController>/UpdateAccount
        /// <summary>
        /// Updates a user account.
        /// </summary>
        /// <param name="model">The model containing the updated user data.</param>
        /// <returns>Returns status 201 (Created) if the account was updated successfully or an error message.</returns>
        /// <response code="201">Returns status 201 (Created) if the account was updated successfully.</response>
        /// <response code="400">If the model state is not valid.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpPut("UpdateAccount")]
        [Authorize]
        public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountRequest model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var query = _dataServiceFactory.CreateQueryUserService();
                    var user = await query.GetUserByIdAsync(
                        User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

                    user.UserName = model.UserName ?? user.UserName;
                    user.Email = model.Email ?? user.Email;
                    user.FirstName = model.FirstName ?? user.FirstName;
                    user.LastName = model.LastName ?? user.LastName;

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

        // DELETE api/<AdminUserController>/DeleteAccount
        /// <summary>
        /// Deletes the user account.
        /// </summary>
        /// <returns>Returns status 204 (No Content) if the account was deleted successfully or an error message.</returns>
        /// <response code="204">Returns status 204 (No Content) if the account was deleted successfully.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpDelete("DeleteAccount")]
        [Authorize]
        public async Task<IActionResult> DeleteAccount()
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryUserService();
                var user = await query.GetUserByIdAsync(
                    User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);


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

        #endregion

    }
}
