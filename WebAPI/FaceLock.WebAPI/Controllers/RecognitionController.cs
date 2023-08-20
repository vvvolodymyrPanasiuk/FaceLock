using FaceLock.WebAPI.Models.AdminUserModels.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using FaceLock.WebAPI.Models.RecognitionModels.Response;
using FaceLock.DataManagement.Services;

namespace FaceLock.WebAPI.Controllers
{
    /// <summary>
    /// Recognition API controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RecognitionController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly Recognition.Services.IFaceRecognitionService<string> _recognitionService;
        private readonly IDataServiceFactory _dataServiceFactory;

        public RecognitionController(ILogger<AuthenticationController> logger,    
            Recognition.Services.IFaceRecognitionService<string> recognitionService,
            IDataServiceFactory dataServiceFactory)
        {
            _logger = logger;
            _recognitionService = recognitionService;
            _dataServiceFactory = dataServiceFactory;
        }


        // POST api/<RecognitionController>/RecognizeUser
        /// <summary>
        /// Recognize user by user's photos.
        /// </summary>
        /// <param name="files">The files to be identification as the user.</param>
        /// <returns>Returns status 200 (Ok) if the identification successfully or an error message.</returns>
        /// <response code="200">Returns status 200 (Ok) if identification successfully.</response>
        /// <response code="204">Returns status 204 (NoContent) if identification faild.</response>
        /// <response code="400">If the model state is not valid.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpPost("RecognizeUser")]
        public async Task<IActionResult> RecognizeUser([FromForm] AddUserPhotosRequest files)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var regonizeResult = new Recognition.DTO.FaceRecognitionResult<string>();
                    foreach (var face in files.Files)
                    {
                        regonizeResult = await _recognitionService.RecognizeUserByFaceAsync(face);
                    }

                    if (regonizeResult.UserId == null)
                    {
                        return StatusCode(StatusCodes.Status204NoContent);
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status200OK, new IdentificationResponse(regonizeResult.UserId, regonizeResult.PredictionDistance));
                    }
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


        // POST api/<RecognitionController>/{placeId}/RecognizeUserForPlace
        /// <summary>
        /// Recognize user by user's photos for place.
        /// </summary>
        /// <param name="placeId">The ID of the place to recognize.</param>
        /// <param name="files">The files to be identification as the user.</param>
        /// <returns>Returns status 200 (Ok) if the identification successfully or an error message.</returns>
        /// <response code="200">Returns status 200 (Ok) if identification successfully.</response>
        /// <response code="204">Returns status 204 (NoContent) if identification faild.</response>
        /// <response code="400">If the model state is not valid.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpPost("{placeId}/RecognizeUserForPlace")]
        public async Task<IActionResult> RecognizeUserForPlace(int placeId, [FromForm] AddUserPhotosRequest files)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var query = _dataServiceFactory.CreateQueryPlaceService();
                    var place = await query.GetPlaceByIdAsync(placeId);

                    var regonizeResult = new Recognition.DTO.FaceRecognitionResult<string>();
                    foreach (var face in files.Files)
                    {
                        regonizeResult = await _recognitionService.RecognizeUserByFaceAsync(face);
                    }

                    if (regonizeResult.UserId == null)
                    {
                        return StatusCode(StatusCodes.Status204NoContent);
                    }
                    else
                    {
                        var command = _dataServiceFactory.CreateCommandPlaceService();
                        await command.AddVisitAsync(
                            new Domain.Entities.PlaceAggregate.Visit 
                            { 
                                PlaceId = placeId,
                                UserId = regonizeResult.UserId,
                                CheckInTime = DateTime.UtcNow
                            });

                        return StatusCode(StatusCodes.Status200OK, new IdentificationResponse(regonizeResult.UserId, regonizeResult.PredictionDistance));
                    }
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


        // POST api/<RecognitionController>/{doorLockId}/RecognizeUserForDoorLock
        /// <summary>
        /// Recognize user by user's photos for door lock.
        /// </summary>
        /// <param name="doorLockId">The ID of the door lock to recognize.</param>
        /// <param name="files">The files to be identification as the user.</param>
        /// <returns>Returns status 200 (Ok) if the identification successfully or an error message.</returns>
        /// <response code="200">Returns status 200 (Ok) if identification successfully.</response>
        /// <response code="204">Returns status 204 (NoContent) if identification faild.</response>
        /// <response code="400">If the model state is not valid.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpPost("{doorLockId}/RecognizeUserForDoorLock")]
        public async Task<IActionResult> RecognizeUserForDoorLock(int doorLockId, [FromForm] AddUserPhotosRequest files)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var query = _dataServiceFactory.CreateQueryDoorLockService();
                    var doorLock = await query.GetDoorLockByIdAsync(doorLockId);
                    var token = await query.GetAccessTokenToDoorLockAsync(doorLockId);

                    var regonizeResult = new Recognition.DTO.FaceRecognitionResult<string>();
                    foreach (var face in files.Files)
                    {
                        regonizeResult = await _recognitionService.RecognizeUserByFaceAsync(face);
                    }

                    if (regonizeResult.UserId == null)
                    {
                        return StatusCode(StatusCodes.Status204NoContent);
                    }
                    else
                    {
                        var command = _dataServiceFactory.CreateCommandDoorLockService();
                        await command.AddDoorLockHistoryAsync(
                            new Domain.Entities.DoorLockAggregate.DoorLockHistory
                            {
                                DoorLockId = doorLock.Id,
                                UserId = regonizeResult.UserId,
                                OpenedDateTime = DateTime.UtcNow
                            });

                        return StatusCode(StatusCodes.Status200OK, new IdentificationResponse(regonizeResult.UserId, regonizeResult.PredictionDistance));
                    }
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
    }
}
