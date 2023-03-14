using FaceLock.Domain.Entities.RoomAggregate;
using FaceLock.Domain.Entities.UserAggregate;
using FaceLock.Domain.Repositories;
using FaceLock.EF.Repositories;
using FaceLock.WebAPI.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
        private readonly IRoomRepository _roomRepository;
        //private readonly IWebHostEnvironment _hostEnvironment; // сервіс для доступу до папки wwwroot

        public UserController(
            UserManager<User> userManager,
            IUserRepository userRepository, 
            IVisitRepository visitRepository, 
            IRoomRepository roomRepository
            //IWebHostEnvironment hostEnvironment
            )
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _visitRepository = visitRepository;
            _roomRepository = roomRepository;
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
        /// Returns a list of visits made to a particular room
        /// </summary>
        /// <param name="roomId">The ID of the room</param>
        /// <returns>A list of visits made to the room</returns>
        [HttpGet("visits/{roomId}")]
        public async Task<ActionResult<List<Visit>>> GetRoomVisits(int roomId)
        {
            // Get the visit with the specified ID from the visit repository
            var visits = await _visitRepository.GetVisitsByRoomIdAsync(roomId);

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
        /// Returns a list of all rooms in the system
        /// </summary>
        /// <returns>A list of all rooms</returns>
        [HttpGet("rooms")]
        public async Task<ActionResult<List<Room>>> GetAllRooms()
        {
            // Call the GetAllAsync method of the injected IRoomRepository to retrieve all rooms.
            var rooms = await _roomRepository.GetAllAsync();

            // If the rooms was not found, return a 404 Not Found response
            if (rooms == null)
            {
                return NotFound();
            }

            // Return an HTTP 200 OK response with the retrieved rooms as the response body.
            return Ok(rooms);
        }


        /// <summary>
        /// Gets a room by its id.
        /// </summary>
        /// <param name="id">The id of the room.</param>
        /// <returns>The room with the given id, or a NotFound response if the room is not found.</returns>
        [HttpGet("rooms/{id}")]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            // Retrieve the room by its id from the database using the RoomRepository.
            var room = await _roomRepository.GetByIdAsync(id);

            // If the room is not found in the database, return a NotFound response.
            if (room == null)
            {
                return NotFound();
            }

            // Return the room in an Ok response.
            return Ok(room);
        }

    }
}
