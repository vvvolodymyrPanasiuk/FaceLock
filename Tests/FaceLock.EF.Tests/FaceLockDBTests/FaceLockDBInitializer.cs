using FaceLock.Domain.Entities.RoomAggregate;
using FaceLock.Domain.Entities.UserAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceLock.EF.Tests.FaceLockDBTests
{
    public static class FaceLockDBInitializer
    {
        public static void SeedData(FaceLockDbContext _context)
        {
            var users = new[]
            {
                new User { Id = "1", FirstName = "John", LastName = "Doe", UserName = "johndoe", Email = "johndoe@example.com" },
                new User { Id = "2", FirstName = "Jane", LastName = "Doe", UserName = "janedoe", Email = "janedoe@example.com" },
                new User { Id = "3", FirstName = "Bob", LastName = "Smith", UserName = "bobsmith", Email = "bobsmith@example.com" }
            };

            _context.Users.AddRange(users);

            var rooms = new[]
            {      
                new Room { Id = 1, Name = "Conference Room 1", NumberRoom = "101" },
                new Room { Id = 2, Name = "Conference Room 2", NumberRoom = "102" },  
                new Room { Id = 3, Name = "Conference Room 3", NumberRoom = "103" }
            };

            _context.Rooms.AddRange(rooms);

            var visits = new[]
            {
                new Visit { Id = 1, UserId = "1", RoomId = 1, CheckInTime = DateTime.Now.AddHours(-2), CheckOutTime = DateTime.Now.AddHours(-1) },
                new Visit { Id = 2, UserId = "2", RoomId = 2, CheckInTime = DateTime.Now.AddHours(-3), CheckOutTime = DateTime.Now.AddHours(-2) },
                new Visit { Id = 3, UserId = "3", RoomId = 3, CheckInTime = DateTime.Now.AddHours(-1) }
            };

            _context.Visits.AddRange(visits);

            _context.SaveChanges();
        }
    }
}
