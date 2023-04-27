using FaceLock.DataManagement.Services;
using FaceLock.WebAPI.Models.UserModels.Request;
using FaceLock.WebAPI.Models.UserModels.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace FaceLock.WebAPI.Controllers
{
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
        /// <returns>Returns status 200 and user information or an error message.</returns>
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
        /// <returns>Returns status 200 and a list of visits or an error message.</returns>
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
                    new UserVisit(u.Id, u.UserId, u.PlaceId, u.CheckInTime, u.CheckOutTime));

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
        /// <returns>Returns a list of UserAccess objects or an error message.</returns>
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


        // GET: api/<DoorLockController>/GetUserHistories
        /// <summary>
        /// Retrieves the user's door lock histories.
        /// </summary>
        /// <returns>Returns status 200 and the user's door lock histories or an error message.</returns>
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

        #endregion


        #region PUT Metods

        // PUT api/<AdminUserController>/UpdateAccount
        /// <summary>
        /// Updates a user account.
        /// </summary>
        /// <param name="model">The model containing the updated user data.</param>
        /// <returns>Returns status 201 if successful or an error message.</returns>
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

        // DELETE api/<AdminUserController>/DeleteAccount
        /// <summary>
        /// Deletes the user account.
        /// </summary>
        /// <returns>Returns status 204 if successful or an error message.</returns>
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
