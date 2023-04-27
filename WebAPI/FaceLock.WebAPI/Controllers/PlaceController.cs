using FaceLock.DataManagement.Services;
using FaceLock.Domain.Entities.UserAggregate;
using FaceLock.WebAPI.Models.AdminUserModels.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Threading.Tasks;
using System;
using FaceLock.WebAPI.Models.PlaceModels.Request;
using FaceLock.WebAPI.Models.AdminUserModels.Response;
using System.Linq;
using FaceLock.WebAPI.Models.PlaceModels.Response;

namespace FaceLock.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaceController : ControllerBase
    {
        private readonly IDataServiceFactory _dataServiceFactory;
        private readonly ILogger<PlaceController> _logger;

        public PlaceController(
            IDataServiceFactory dataServiceFactory,
            ILogger<PlaceController> logger)
        {
            _dataServiceFactory = dataServiceFactory;
            _logger = logger;
        }


        #region POST Metods

        // POST: api/<PlaceController>/CreatePlace
        /// <summary>
        /// Creates a new place with the given details.
        /// </summary>
        /// <param name="model">The details of the place to be created.</param>
        /// <returns>Returns status 201 if successful or an error message if not.</returns>
        [HttpPost("CreatePlace")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreatePlace([FromBody] CreatePlaceRequest model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var command = _dataServiceFactory.CreateCommandPlaceService();
                    await command.AddPlaceAsync(new Domain.Entities.PlaceAggregate.Place
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

        #endregion


        #region GET Metods

        // GET api/<PlaceController>/GetPlaces
        /// <summary>
        /// Retrieves a list of all places.
        /// </summary>
        /// <returns>Returns a list of GetPlacesResponse objects or an error message.</returns>
        [HttpGet("GetPlaces")]
        [Authorize]
        public async Task<IActionResult> GetPlaces()
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryPlaceService();
                var places = await query.GetAllPlacesAsync();

                var result = places.Select(u =>
                    new GetPlaceResponse(u.Id, u.Name, u.Description));

                return StatusCode(StatusCodes.Status200OK, new GetPlacesResponse(result));
            }
            catch (Exception ex)
            {
                // Log error and return 500 response
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }


        // GET api/<PlaceController>/GetPlace/{placeId}
        /// <summary>
        /// Retrieves the place with the specified ID from the repository.
        /// </summary>
        /// <param name="placeId">The ID of the place to retrieve.</param>
        /// <returns>Returns status 200 and the place data or 404 
        /// if the user is not found or 500 if an error occurred.</returns>
        [HttpGet("GetPlace/{placeId}")]
        [Authorize]
        public async Task<IActionResult> GetPlace(int placeId)
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryPlaceService();
                var place = await query.GetPlaceByIdAsync(placeId);

                return StatusCode(StatusCodes.Status200OK,
                    new GetPlaceResponse(place.Id, place.Name, place.Description));
            }
            catch (Exception ex)
            {
                // Log error and return 500 response
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }


        // GET api/<PlaceController>/GetVisitsByPlaceId/{placeId}
        /// <summary>
        /// Retrieves a list of all visit by place ID.
        /// </summary>
        /// <param name="placeId">The ID of the place to retrieve.</param>
        /// <returns>Returns a list of GetVisitsResponse objects or an error message.</returns>
        [HttpGet("GetVisitsByPlaceId/{placeId}")]
        [Authorize]
        public async Task<IActionResult> GetVisitsByPlaceId(int placeId)
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryPlaceService();
                var visits = await query.GetVisitsByPlaceIdAsync(placeId);

                var result = visits.Select(u =>
                    new UserVisit(u.Id, u.UserId, u.PlaceId, u.CheckInTime, u.CheckOutTime));

                return StatusCode(StatusCodes.Status200OK, new GetVisitsResponse(result));
            }
            catch (Exception ex)
            {
                // Log error and return 500 response
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }


        // GET api/<PlaceController>/GetVisitsByUserId/{userId}
        /// <summary>
        /// Retrieves a list of all visit by user ID.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve.</param>
        /// <returns>Returns a list of GetVisitsResponse objects or an error message.</returns>
        [HttpGet("GetVisitsByUserId/{placeId}")]
        [Authorize]
        public async Task<IActionResult> GetVisitsByUserId(string userId)
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryPlaceService();
                var visits = await query.GetVisitsByUserIdAsync(userId);

                var result = visits.Select(u =>
                    new UserVisit(u.Id, u.UserId, u.PlaceId, u.CheckInTime, u.CheckOutTime));

                return StatusCode(StatusCodes.Status200OK, new GetVisitsResponse(result));
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

        // PUT api/<PlaceController>/UpdatePlace/{placeId}
        /// <summary>
        /// Updates a place by ID.
        /// </summary>
        /// <param name="placeId">The ID of the place to update.</param>
        /// <param name="model">The model containing the updated place data.</param>
        /// <returns>Returns status 201 if successful or an error message.</returns>
        [HttpPut("UpdatePlace/{placeId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePlace(int placeId, [FromBody] UpdatePlaceRequest model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var query = _dataServiceFactory.CreateQueryPlaceService();
                    var place = await query.GetPlaceByIdAsync(placeId);

                    place.Name = place.Name ?? place.Name;
                    place.Description = model.Description ?? place.Description;

                    var command = _dataServiceFactory.CreateCommandPlaceService();
                    await command.UpdatePlaceAsync(place);

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

        // DELETE api/<PlaceController>/DeletePlace/{placeId}
        /// <summary>
        /// Deletes the place with the specified ID.
        /// </summary>
        /// <param name="placeId">The ID of the place to delete.</param>
        /// <returns>Returns status 204 if successful or an error message.</returns>
        [HttpDelete("DeleteUser/{placeId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePlace(int placeId)
        {
            try
            {
                var query = _dataServiceFactory.CreateQueryPlaceService();
                var place = await query.GetPlaceByIdAsync(placeId);

                var command = _dataServiceFactory.CreateCommandPlaceService();
                await command.DeletePlaceAsync(place);

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
