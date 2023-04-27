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

namespace FaceLock.WebAPI.Controllers
{
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
        /// <returns>Returns status 201 if successful or an error message if not.</returns>
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
        /// <returns>Returns status 201 if successful or an error message if not.</returns>
        [HttpPost("CreateAccessDoorLock")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAccessDoorLock([FromBody] CreateAccessDoorLockRequest model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var command = _dataServiceFactory.CreateCommandDoorLockService();
                    await command.AddDoorLockAccessAsync(new Domain.Entities.DoorLockAggregate.UserDoorLockAccess
                    {
                        UserId = model.UserId,
                        DoorLockId = model.DoorLockId,
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

        #endregion


        #region GET Metods

        // GET api/<DoorLockController>/GetDoorLocks
        /// <summary>
        /// Retrieves a list of all door lock.
        /// </summary>
        /// <returns>Returns a list of GetDoorLocksResponse objects or an error message.</returns>
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


        // GET api/<PlaceController>/GetDoorLock/{doorLockId}
        /// <summary>
        /// Retrieves the door lock with the specified ID from the repository.
        /// </summary>
        /// <param name="doorLockId">The ID of the door lock to retrieve.</param>
        /// <returns>Returns status 200 and the door lock data or 404 
        /// if the door lock is not found or 500 if an error occurred.</returns>
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


        // GET api/<PlaceController>/GetDoorLockTokens/{doorLockId}
        /// <summary>
        /// Retrieves the door lock tokens with the specified ID from the repository.
        /// </summary>
        /// <param name="doorLockId">The ID of the door lock to retrieve.</param>
        /// <returns>Returns status 200 and the door lock tokens data or 404 
        /// if the door lock is not found or 500 if an error occurred.</returns>
        [HttpGet("GetDoorLockTokens/{doorLockId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDoorLockTokens(int doorLockId)
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryDoorLockService();
                var doorLockTokens = await query.GetAccessTokensByDoorLockIdAsync(doorLockId);

                var result = doorLockTokens.Select(u =>
                    new DoorLockToken(u.Id, u.DoorLockId, u.AccessToken, u.Utilized));

                return StatusCode(StatusCodes.Status200OK, new GetDoorLockTokensResponse(result));
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
        /// <returns>Returns a list of GetAccessesDoorLockResponse objects or an error message.</returns>
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
        /// <returns>Returns a list of GetAccessesDoorLockResponse objects or an error message.</returns>
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
        /// <returns>Returns a list of GetDoorLockHistoryResponse objects or an error message.</returns>
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
        /// <returns>Returns a list of GetDoorLockHistoryResponse objects or an error message.</returns>
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
        /// <returns>Returns status 201 if successful or an error message.</returns>
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
        /// <returns>Returns status 201 if successful or an error message.</returns>
        [HttpPut("UpdateAccessDoorLock")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAccessDoorLock([FromBody] UpdateAccessDoorLockRequest model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var query = _dataServiceFactory.CreateQueryDoorLockService();
                    var accessesDoorLock = await query.GetAccessByDoorLockIdAsync(model.DoorLockId);
                    var accessDoorLock = accessesDoorLock.FirstOrDefault(a => a.UserId == model.UserId);

                    accessDoorLock.UserId = model.UserId ?? accessDoorLock.UserId;
                    accessDoorLock.DoorLockId = model.DoorLockId;
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

        #endregion


        #region DELETE Metods

        // DELETE api/<DoorLockController>/DeleteDoorLock/{placeId}
        /// <summary>
        /// Deletes the door lock with the specified ID.
        /// </summary>
        /// <param name="doorLockId">The ID of the door lock to delete.</param>
        /// <returns>Returns status 204 if successful or an error message.</returns>
        [HttpDelete("DeleteDoorLock/{placeId}")]
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
        /// <returns>Returns status 204 if successful or an error message.</returns>
        [HttpDelete("{doorLockId}/DeleteAccessDoorLock/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAccessDoorLock(int doorLockId, string userId)
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryDoorLockService();
                var accessesDoorLock = await query.GetAccessByDoorLockIdAsync(doorLockId);

                var accessForDelete = accessesDoorLock.FirstOrDefault(a => a.UserId == userId);

                if (accessForDelete == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Access not exist.");
                }

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
