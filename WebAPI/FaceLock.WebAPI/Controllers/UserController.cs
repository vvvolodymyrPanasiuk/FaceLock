using FaceLock.Domain.Entities.PlaceAggregate;
using FaceLock.Domain.Entities.UserAggregate;
using FaceLock.Domain.Repositories.PlaceRepository;
using FaceLock.Domain.Repositories.UserRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace FaceLock.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager; // сервіс для керування користувачами
        private readonly IUserRepository _userRepository;
        private readonly IVisitRepository _visitRepository;
        private readonly IPlaceRepository _placeRepository;
        //private readonly IWebHostEnvironment _hostEnvironment; // сервіс для доступу до папки wwwroot

        public UserController(
            UserManager<User> userManager,
            IUserRepository userRepository, 
            IVisitRepository visitRepository, 
            IPlaceRepository placeRepository
            //IWebHostEnvironment hostEnvironment
            )
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _visitRepository = visitRepository;
            _placeRepository = placeRepository;
            //_hostEnvironment = hostEnvironment;
        }

        // TODO: Добавити перегляд відвідувач як коричстувачів

        /// <summary>
        /// Returns a list of all visits in the system
        /// </summary>
        /// <returns>A list of all visits</returns>
        [HttpGet("visits")]
        public async Task<ActionResult<List<Visit>>> GetAllVisits()
        {
            // Get the visit with the specified ID from the visit repository
            var visits = await _visitRepository.GetAllAsync();

            // If the visit was not found, return a 404 Not Found response
            if (visits == null)
            {
                return NotFound();
            }

            // Return the visit in an Ok response
            return Ok(visits);
        }


        /// <summary>
        /// Returns a list of visits made by a particular user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>A list of visits made by the user</returns>
        [HttpGet("visits/{userId}")]
        public async Task<ActionResult<List<Visit>>> GetUserVisits(string userId)
        {
            // Get the visit with the specified ID from the visit repository
            var visits = await _visitRepository.GetVisitsByUserIdAsync(userId);

            // If the visit was not found, return a 404 Not Found response
            if (visits == null)
            {
                return NotFound();
            }

            // Return the visit in an Ok response
            return Ok(visits);
        }


        /// <summary>
        /// Returns a list of visits made to a particular place
        /// </summary>
        /// <param name="placeId">The ID of the place</param>
        /// <returns>A list of visits made to the place</returns>
        [HttpGet("visits/{placeId}")]
        public async Task<ActionResult<List<Visit>>> GetPlaceVisits(int placeId)
        {
            // Get the visit with the specified ID from the visit repository
            var visits = await _visitRepository.GetVisitsByPlaceIdAsync(placeId);

            // If the visit was not found, return a 404 Not Found response
            if (visits == null)
            {
                return NotFound();
            }

            // Return the visit in an Ok response
            return Ok(visits);
        }


        /// <summary>
        /// Returns a specific visit by its ID
        /// </summary>
        /// <param name="visitId">The ID of the visit</param>
        /// <returns>The visit with the specified ID</returns>
        [HttpGet("visits/{visitId}")]
        public async Task<ActionResult<Visit>> GetVisit(int visitId)
        {
            // Get the visit with the specified ID from the visit repository
            var visits = await _visitRepository.GetByIdAsync(visitId);

            // If the visit was not found, return a 404 Not Found response
            if (visits == null)
            {
                return NotFound();
            }

            // Return the visit in an Ok response
            return Ok(visits);
        }


        /// <summary>
        /// Returns a list of all places in the system
        /// </summary>
        /// <returns>A list of all places</returns>
        [HttpGet("places")]
        public async Task<ActionResult<List<Place>>> GetAllPlaces()
        {
            // Call the GetAllAsync method of the injected IPlaceRepository to retrieve all places.
            var places = await _placeRepository.GetAllAsync();

            // If the places was not found, return a 404 Not Found response
            if (places == null)
            {
                return NotFound();
            }

            // Return an HTTP 200 OK response with the retrieved places as the response body.
            return Ok(places);
        }


        /// <summary>
        /// Gets a place by its id.
        /// </summary>
        /// <param name="id">The id of the place.</param>
        /// <returns>The place with the given id, or a NotFound response if the place is not found.</returns>
        [HttpGet("places/{id}")]
        public async Task<ActionResult<Place>> GetPlace(int id)
        {
            // Retrieve the place by its id from the database using the PlaceRepository.
            var place = await _placeRepository.GetByIdAsync(id);

            // If the place is not found in the database, return a NotFound response.
            if (place == null)
            {
                return NotFound();
            }

            // Return the place in an Ok response.
            return Ok(place);
        }

    }
}
