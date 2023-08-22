using FaceLock.WebAPI.Models.AdminUserModels.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using FaceLock.WebAPI.Models.RecognitionModels.Response;
using FaceLock.DataManagement.Services;
using FaceLock.WebSocket.Protos;
using FaceLock.WebAPI.GrpcClientFactory;

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
        private readonly IGrpcClientChannelFactory _grpcClientChannelFactory;

        public RecognitionController(ILogger<AuthenticationController> logger,    
            Recognition.Services.IFaceRecognitionService<string> recognitionService,
            IDataServiceFactory dataServiceFactory,
            IGrpcClientChannelFactory grpcClientChannelFactory)
        {
            _logger = logger;
            _recognitionService = recognitionService;
            _dataServiceFactory = dataServiceFactory;
            _grpcClientChannelFactory = grpcClientChannelFactory;
        }


        // POST api/<RecognitionController>/RecognizeUser
        /// <summary>
        /// Recognize user by user's photos.
        /// </summary>
        /// <param name="files">The files to be identification as the user.</param>
        /// <returns>Returns status 200 (Ok) if the identification successfully or an error message.</returns>
        /// <responseGrpsServe code="200">Returns status 200 (Ok) if identification successfully.</responseGrpsServe>
        /// <responseGrpsServe code="204">Returns status 204 (NoContent) if identification faild.</responseGrpsServe>
        /// <responseGrpsServe code="400">If the model state is not valid.</responseGrpsServe>
        /// <responseGrpsServe code="500">If an error occurred during the operation.</responseGrpsServe>
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
						var queryUserService = _dataServiceFactory.CreateQueryUserService();
						var user = await queryUserService.GetUserByIdAsync(regonizeResult.UserId);
                        return StatusCode(StatusCodes.Status200OK, new IdentificationResponse(user.Id, user.UserName, user.Email, user.FirstName, user.LastName, user.Status, regonizeResult.PredictionDistance));
                    }
                }
                catch (ArgumentNullException ex)
                {
                    // Log error and return 400 responseGrpsServe
                    _logger.LogError($"Error: {ex.Message}");
                    return StatusCode(StatusCodes.Status400BadRequest, $"Error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    // Log error and return 500 responseGrpsServe
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
        /// <responseGrpsServe code="200">Returns status 200 (Ok) if identification successfully.</responseGrpsServe>
        /// <responseGrpsServe code="204">Returns status 204 (NoContent) if identification faild.</responseGrpsServe>
        /// <responseGrpsServe code="400">If the model state is not valid.</responseGrpsServe>
        /// <responseGrpsServe code="500">If an error occurred during the operation.</responseGrpsServe>
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
						var queryUserService = _dataServiceFactory.CreateQueryUserService();
						var user = await queryUserService.GetUserByIdAsync(regonizeResult.UserId);
                        return StatusCode(StatusCodes.Status200OK, new IdentificationResponse(user.Id, user.UserName, user.Email, user.FirstName, user.LastName, user.Status, regonizeResult.PredictionDistance));
					}
                }
                catch (ArgumentNullException ex)
                {
                    // Log error and return 400 responseGrpsServe
                    _logger.LogError($"Error: {ex.Message}");
                    return StatusCode(StatusCodes.Status400BadRequest, $"Error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    // Log error and return 500 responseGrpsServe
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
        /// <responseGrpsServe code="200">Returns status 200 (Ok) if identification successfully.</responseGrpsServe>
        /// <responseGrpsServe code="204">Returns status 204 (NoContent) if identification faild.</responseGrpsServe>
        /// <responseGrpsServe code="400">If the model state is not valid.</responseGrpsServe>
        /// <responseGrpsServe code="500">If an error occurred during the operation.</responseGrpsServe>
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



                    int responseGrpsServeStatus;
                    string responseGrpsServeMessage;
                    // TODO: upd responseGrpsServe
                    using (var channel = _grpcClientChannelFactory.CreateGrpcClientChannel())
                    {
                        var client = new DoorLock.DoorLockClient(channel);
                        var requestGrpsServe = new DoorLockServiceRequest
                        {
                            Token = token
                        };

                        var responseGrpsServe = await client.OpenDoorLockAsync(requestGrpsServe);
                        responseGrpsServeStatus = responseGrpsServe.Status;
                        responseGrpsServeMessage = responseGrpsServe.Message;

                        if (responseGrpsServe != null)
                        {
                            return StatusCode(StatusCodes.Status200OK, responseGrpsServe);
                        }
                    }
                    

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

						var queryUserService = _dataServiceFactory.CreateQueryUserService();
						var user = await queryUserService.GetUserByIdAsync(regonizeResult.UserId);
						return StatusCode(StatusCodes.Status200OK, new IdentificationResponse(user.Id, user.UserName, user.Email, user.FirstName, user.LastName, user.Status, regonizeResult.PredictionDistance));
					}
                }
                catch (ArgumentNullException ex)
                {
                    // Log error and return 400 responseGrpsServe
                    _logger.LogError($"Error: {ex.Message}");
                    return StatusCode(StatusCodes.Status400BadRequest, $"Error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    // Log error and return 500 responseGrpsServe
                    _logger.LogError($"Error: {ex.Message}");
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
                }
            }
            return BadRequest(ModelState);
        }
    }
}
