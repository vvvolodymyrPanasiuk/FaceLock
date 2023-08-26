using FaceLock.DataManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using FaceLock.WebAPI.Models.DoorLockModels.Request;
using System.Linq;
using FaceLock.WebAPI.Models.DoorLockModels.Response;
using Microsoft.AspNetCore.Http.HttpResults;
using FaceLock.Domain.Entities.DoorLockAggregate;

namespace FaceLock.WebAPI.Controllers
{
    /// <summary>
    /// Door lock API controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DoorLockController : ControllerBase
    {
        private readonly IDataServiceFactory _dataServiceFactory;
        private readonly ILogger<PlaceController> _logger;

        public DoorLockController(
            IDataServiceFactory dataServiceFactory,
            ILogger<PlaceController> logger)
        {
            _dataServiceFactory = dataServiceFactory;
            _logger = logger;
        }


        #region POST Metods

        // POST: api/<DoorLockController>/CreateDoorLock
        /// <summary>
        /// Creates a new door lock with the given details.
        /// </summary>
        /// <param name="model">The details of the door lock to be created.</param>
        /// <returns>Returns status 201 (Created) if the door lock was created successfully or an error message.</returns>
        /// <response code="201">Returns status 201 (Created) if the door lock was created successfully.</response>
        /// <response code="400">If the model state is not valid.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpPost("CreateDoorLock")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateDoorLock([FromBody] CreateDoorLockRequest model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var command = _dataServiceFactory.CreateCommandDoorLockService();
                    await command.AddDoorLockAsync(new Domain.Entities.DoorLockAggregate.DoorLock
                    {
                        Name = model.Name,
                        Description = model.Description
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


        // POST: api/<DoorLockController>/CreateAccessDoorLock
        /// <summary>
        /// Creates a new access to door lock.
        /// </summary>
        /// <param name="model">The access of door lock to be created.</param>
        /// <returns>Returns status 201 (Created) if the user's access to a door lock was created successfully or an error message.</returns>
        /// <response code="201">Returns status 201 (Created) if the user's access to a door lock was created successfully.</response>
        /// <response code="400">If the model state is not valid.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpPost("CreateAccessDoorLock")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAccessDoorLock([FromBody] CreateAccessDoorLockRequest model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var queryUser = _dataServiceFactory.CreateQueryUserService();
                    var user = await queryUser.GetUserByIdAsync(model.UserId);

                    var queryDoorLock = _dataServiceFactory.CreateQueryDoorLockService();
                    var doorLock = await queryDoorLock.GetDoorLockByIdAsync(model.DoorLockId);

                    var queryDoorLockAccess = _dataServiceFactory.CreateQueryDoorLockService();
                    if(await queryDoorLockAccess.IsExistUserDoorLockAccessByIds(model.UserId, model.DoorLockId))
                    {
                        return StatusCode(StatusCodes.Status409Conflict, $"Access already exists");
                    }

                    var commandDoorLock = _dataServiceFactory.CreateCommandDoorLockService();
                    await commandDoorLock.AddDoorLockAccessAsync(new Domain.Entities.DoorLockAggregate.UserDoorLockAccess
                    {
                        UserId = user.Id,
                        DoorLockId = doorLock.Id,
                        HasAccess = model.HasAccess
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


        // POST: api/<DoorLockController>/CreateSecretInfoDoorLock
        /// <summary>
        /// Creates a new secret info to door lock.
        /// </summary>
        /// <param name="model">The secret info of door lock to be created.</param>
        /// <returns>Returns status 201 (Created) if the user's secret info to a door lock was created successfully or an error message.</returns>
        /// <response code="201">Returns status 201 (Created) if the user's access to a door lock was created successfully.</response>
        /// <response code="400">If the model state is not valid.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpPost("CreateSecretInfoDoorLock")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateSecretInfoDoorLock([FromBody] CreateSecretInfoDoorLockRequest model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var queryDoorLock = _dataServiceFactory.CreateQueryDoorLockService();
                    var doorLock = await queryDoorLock.GetDoorLockByIdAsync(model.DoorLockId);

                    var commandDoorLock = _dataServiceFactory.CreateCommandDoorLockService();
                    await commandDoorLock.CreateSecurityInfoAsync(doorLock.Id, model.UrlConnection);

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

        // GET api/<DoorLockController>/GetDoorLocks
        /// <summary>
        /// Retrieves a list of all door lock.
        /// </summary>
        /// <returns>Returns status 200 (OK) with a list of door locks or an error message.</returns>
        /// <response code="200">Returns status 200 (OK) with a list of door locks.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetDoorLocksResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpGet("GetDoorLocks")]
        [Authorize]
        public async Task<IActionResult> GetDoorLocks()
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryDoorLockService();
                var doorLocks = await query.GetAllDoorLocksAsync();

                var result = doorLocks.Select(u =>
                    new GetDoorLockResponse(u.Id, u.Name, u.Description));

                return StatusCode(StatusCodes.Status200OK, new GetDoorLocksResponse(result));
            }
            catch (Exception ex)
            {
                // Log error and return 500 response
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }


        // GET api/<DoorLockController>/GetDoorLock/{doorLockId}
        /// <summary>
        /// Retrieves the door lock with the specified ID from the repository.
        /// </summary>
        /// <param name="doorLockId">The ID of the door lock to retrieve.</param>
        /// <returns>Returns status 200 (OK) and details of the door lock if successful or an error message.</returns>
        /// <response code="200">Returns status 200 (OK) and details of the door lock if successful.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="404">If a door lock with the specified ID does not exist.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetDoorLockResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpGet("GetDoorLock/{doorLockId}")]
        [Authorize]
        public async Task<IActionResult> GetDoorLock(int doorLockId)
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryDoorLockService();
                var doorLock = await query.GetDoorLockByIdAsync(doorLockId);

                return StatusCode(StatusCodes.Status200OK,
                    new GetDoorLockResponse(doorLock.Id, doorLock.Name, doorLock.Description));
            }
            catch (Exception ex)
            {
                // Log error and return 500 response
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }


        // GET api/<DoorLockController>/GetDoorLockSecretInfo
        /// <summary>
        /// Retrieves the door lock tokens with the specified ID from the repository.
        /// </summary>
        /// <param name="doorLockId">The ID of the door lock to retrieve.</param>
        /// <returns>Returns status 200 (OK) and a list of access tokens associated with the given door lock ID.</returns>
        /// <response code="200">Returns status 200 (OK) and a list of access tokens associated with the given door lock ID.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="404">If the door lock with the given ID does not exist.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetDoorLockSecretInfoResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpGet("GetDoorLockSecretInfo")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDoorLockSecretInfo(int doorLockId)
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryDoorLockService();
                var doorLockSecretInfo = await query.GetSecurityInfoByDoorLockIdAsync(doorLockId);

                return StatusCode(StatusCodes.Status200OK, 
                    new GetDoorLockSecretInfoResponse(doorLockSecretInfo.DoorLockId, doorLockSecretInfo.DoorLockId, doorLockSecretInfo.UrlConnection, doorLockSecretInfo.SecretKey));
            }
            catch (Exception ex)
            {
                // Log error and return 500 response
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }


        // GET api/<DoorLockController>/GetUserAccessesByDoorLockId/{doorLockId}
        /// <summary>
        /// Retrieves a list of all user accesses to door lock by door lock ID.
        /// </summary>
        /// <param name="doorLockId">The ID of the door lock to retrieve.</param>
        /// <returns>Returns the list of user accesses for the given door lock.</returns>
        /// <response code="200">Returns the list of user accesses for the given door lock.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAccessesDoorLockResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpGet("GetUserAccessesByDoorLockId/{doorLockId}")]
        [Authorize]
        public async Task<IActionResult> GetUserAccessesByDoorLockId(int doorLockId)
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryDoorLockService();
                var accessesDoorLock = await query.GetAccessByDoorLockIdAsync(doorLockId);

                var result = accessesDoorLock.Select(u =>
                    new UserAccessDoorLock(u.Id, u.UserId, u.DoorLockId, u.HasAccess));

                return StatusCode(StatusCodes.Status200OK, new GetAccessesDoorLockResponse(result));
            }
            catch (Exception ex)
            {
                // Log error and return 500 response
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }


        // GET api/<DoorLockController>/GetUserAccessesByUserId/{userId}
        /// <summary>
        /// Retrieves a list of all user accesses to door lock by door lock ID.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve.</param>
        /// <returns>Returns status 200 (OK) and the door lock accesses for the given user, or an error message.</returns>
        /// <response code="200">Returns status 200 (OK) and the door lock accesses for the given user.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAccessesDoorLockResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpGet("GetUserAccessesByUserId/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUserAccessesByUserId(string userId)
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryDoorLockService();
                var accessesDoorLock = await query.GetAccessByUserIdAsync(userId);

                var result = accessesDoorLock.Select(u =>
                    new UserAccessDoorLock(u.Id, u.UserId, u.DoorLockId, u.HasAccess));

                return StatusCode(StatusCodes.Status200OK, new GetAccessesDoorLockResponse(result));
            }
            catch (Exception ex)
            {
                // Log error and return 500 response
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }


        // GET api/<DoorLockController>/GetDoorLockHistoryByDoorLockId/{doorLockId}
        /// <summary>
        /// Retrieves a list of all history to door lock by door lock ID.
        /// </summary>
        /// <param name="doorLockId">The ID of the door lock to retrieve.</param>
        /// <returns>Returns status 200 (OK) and the door lock history or an error message.</returns>
        /// <response code="200">Returns status 200 (OK) and the door lock history.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="404">If a door lock with the given ID does not exist.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetDoorLockHistoryResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpGet("GetDoorLockHistoryByDoorLockId/{doorLockId}")]
        [Authorize]
        public async Task<IActionResult> GetDoorLockHistoryByDoorLockId(int doorLockId)
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryDoorLockService();
                var historiesDoorLock = await query.GetDoorLockHistoryByDoorLockIdAsync(doorLockId);

                var result = historiesDoorLock.Select(u =>
                    new HistoryDoorLock(u.Id, u.UserId, u.DoorLockId, u.OpenedDateTime));

                return StatusCode(StatusCodes.Status200OK, new GetDoorLockHistoryResponse(result));
            }
            catch (Exception ex)
            {
                // Log error and return 500 response
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }


        // GET api/<DoorLockController>/GetDoorLockHistoryByUserId/{userId}
        /// <summary>
        /// Retrieves a list of all history to door lock by door lock ID.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve.</param>
        /// <returns>Returns the door lock history for the specified user ID or an error message.</returns>
        /// <response code="200">Returns the door lock history for the specified user ID.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="404">If no door lock history is found for the specified user ID.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetDoorLockHistoryResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpGet("GetDoorLockHistoryByUserId/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetDoorLockHistoryByUserId(string userId)
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryDoorLockService();
                var historiesDoorLock = await query.GetDoorLockHistoryByUserIdAsync(userId);

                var result = historiesDoorLock.Select(u =>
                    new HistoryDoorLock(u.Id, u.UserId, u.DoorLockId, u.OpenedDateTime));

                return StatusCode(StatusCodes.Status200OK, new GetDoorLockHistoryResponse(result));
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

        // PUT api/<DoorLockController>/UpdateDoorLock/{doorLockId}
        /// <summary>
        /// Updates a door lock by ID.
        /// </summary>
        /// <param name="doorLockId">The ID of the door lock to update.</param>
        /// <param name="model">The model containing the updated door lock data.</param>
        /// <returns>Returns status 201 (Created) if the door lock was updated successfully or an error message.</returns>
        /// <response code="201">Returns status 201 (Created) if the door lock was updated successfully.</response>
        /// <response code="400">If the model state is not valid.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="404">If the door lock with the given ID is not found.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpPut("UpdateDoorLock/{doorLockId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDoorLock(int doorLockId, [FromBody] UpdateDoorLockRequest model)
        {
            if (ModelState.IsValid)
            {
                try
                {                  
                    var query = _dataServiceFactory.CreateQueryDoorLockService();
                    var doorLock = await query.GetDoorLockByIdAsync(doorLockId);

                    doorLock.Name = model.Name ?? doorLock.Name;
                    doorLock.Description = model.Description ?? doorLock.Description;

                    var command = _dataServiceFactory.CreateCommandDoorLockService();
                    await command.UpdateDoorLockAsync(doorLock);

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


        // PUT api/<DoorLockController>/UpdateAccessDoorLock
        /// <summary>
        /// Updates a user access to door lock.
        /// </summary>
        /// <param name="model">The model containing the updated user access to door lock data.</param>
        /// <returns>Returns status 201 (Created) if the access level was updated successfully or an error message.</returns>
        /// <response code="201">Returns status 201 (Created) if the access level was updated successfully.</response>
        /// <response code="400">If the model state is not valid.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpPut("UpdateAccessDoorLock")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAccessDoorLock([FromBody] UpdateAccessDoorLockRequest model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var query = _dataServiceFactory.CreateQueryDoorLockService();
                    var accessDoorLock = await query.GetUserDoorLockAccessByIdsAsync(model.UserId, model.DoorLockId);

                    accessDoorLock.HasAccess = model.HasAccess;

                    var command = _dataServiceFactory.CreateCommandDoorLockService();
                    await command.UpdateDoorLockAccessAsync(accessDoorLock);

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


        // PUT api/<DoorLockController>/UpdateSecretInfoDoorLock
        /// <summary>
        /// Updates a user secret info to door lock.
        /// </summary>
        /// <param name="model">The model containing the updated secret info to door lock data.</param>
        /// <returns>Returns status 201 (Created) if the secret info was updated successfully or an error message.</returns>
        /// <response code="201">Returns status 201 (Created) if the access level was updated successfully.</response>
        /// <response code="400">If the model state is not valid.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpPut("UpdateSecretInfoDoorLock")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSecretInfoDoorLock([FromBody] UpdateSecretInfoDoorLockRequest model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var query = _dataServiceFactory.CreateQueryDoorLockService();
                    var secretKeyDoorLock = await query.GetSecurityInfoByDoorLockIdAsync(model.Id);

                    secretKeyDoorLock.UrlConnection = model.UrlConnection ?? secretKeyDoorLock.UrlConnection;
                    secretKeyDoorLock.SecretKey = model.SecretKey ?? secretKeyDoorLock.SecretKey;

                    var command = _dataServiceFactory.CreateCommandDoorLockService();
                    await command.UpdateSecurityInfoAsync(secretKeyDoorLock);

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

        // DELETE api/<DoorLockController>/DeleteDoorLock/{doorLockId}
        /// <summary>
        /// Deletes the door lock with the specified ID.
        /// </summary>
        /// <param name="doorLockId">The ID of the door lock to delete.</param>
        /// <returns>Returns status 204 (No Content) if the door lock was deleted successfully or an error message.</returns>
        /// <response code="204">Returns status 204 (No Content) if the door lock was deleted successfully.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="404">If the door lock with the given ID was not found.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpDelete("DeleteDoorLock/{doorLockId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDoorLock(int doorLockId)
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryDoorLockService();
                var doorLock = await query.GetDoorLockByIdAsync(doorLockId);

                var command = _dataServiceFactory.CreateCommandDoorLockService();
                await command.DeleteDoorLockAsync(doorLock);

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                // Log error and return 500 response
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }


        // DELETE api/<DoorLockController>/{doorLockId}/DeleteAccessDoorLock/{userId}
        /// <summary>
        /// Deletes the user access to door lock with the specified ID.
        /// </summary>
        /// <param name="doorLockId">The ID of the door lock to delete.</param>        
        /// <param name="userkId">The ID of the user to delete.</param>
        /// <returns>Returns status 204 (No Content) if the access was deleted successfully or an error message.</returns>
        /// <response code="204">Returns status 204 (No Content) if the access was deleted successfully.</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="404">If the specified user or door lock does not exist.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpDelete("{doorLockId}/DeleteAccessDoorLock/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAccessDoorLock(int doorLockId, string userId)
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryDoorLockService();
                var accessForDelete = await query.GetUserDoorLockAccessByIdsAsync(userId, doorLockId);

                var command = _dataServiceFactory.CreateCommandDoorLockService();
                await command.DeleteDoorLockAccessAsync(accessForDelete);

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
