using FaceLock.WebAPI.Models.AdminUserModels.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using FaceLock.WebAPI.Models.RecognitionModels.Response;

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

        public RecognitionController(ILogger<AuthenticationController> logger,    
            Recognition.Services.IFaceRecognitionService<string> recognitionService)
        {
            _logger = logger;
            _recognitionService = recognitionService;
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

        //public Task<IActionResult> IdentificationForDoor(IFormFileCollection files, string DoorLockId) { throw null; }
        //public Task<IActionResult> IdentificationForPlace(IFormFileCollection files, string PlaceId) { throw null; }
        //public Task<IActionResult> Identification(IFormFileCollection files) { throw null; }
    }
}
