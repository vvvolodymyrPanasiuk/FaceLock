using FaceLock.Domain.Entities.PlaceAggregate;
using FaceLock.Domain.Entities.UserAggregate;


namespace FaceLock.EF.Tests.FaceLockDBTests
{
    public static class FaceLockDBInitializer
    {
        public static void SeedData(FaceLockDbContext _context)
        {
            var users = new[]
            {
                new User 
                { 
                    Id = "1", 
                    FirstName = "John", 
                    LastName = "Doe", 
                    UserName = "johndoe", 
                    Email = "johndoe@example.com",
                    NormalizedEmail = "JOHNDOE@EXAMPLE.COM",
                    NormalizedUserName ="JOHNDOE",
                    PasswordHash = "jdfsavbhui",
                    Status ="Admin"
                },
                new User 
                { 
                    Id = "2", 
                    FirstName = "Jane", 
                    LastName = "Doe", 
                    UserName = "janedoe", 
                    Email = "janedoe@example.com",
                    NormalizedEmail = "JANEDOE@EXAMPLE.COM",
                    NormalizedUserName ="JANEDOE",
                    PasswordHash = "jdfsafdbufsdiovbhui",
                    Status ="User"
                },
                new User 
                { 
                    Id = "3", 
                    FirstName = "Bob", 
                    LastName = "Smith", 
                    UserName = "bobsmith", 
                    Email = "bobsmith@example.com",
                    NormalizedEmail = "BOBSMITH@EXAMPLE.COM",
                    NormalizedUserName ="BOBSMITH",
                    PasswordHash = "ijbhofsd;jbospdf",
                    Status ="User"
                }
            };

            _context.Users.AddRange(users);

            var places = new[]
            {      
                new Place { Id = 1, Name = "Conference Room 1", Description = "101" },
                new Place { Id = 2, Name = "Conference Room 2", Description = "102" },  
                new Place { Id = 3, Name = "Conference Room 3", Description = "103" }
            };

            _context.Places.AddRange(places);

            var visits = new[]
            {
                new Visit { Id = 1, UserId = "1", PlaceId = 1, CheckInTime = DateTime.Now.AddHours(-2), CheckOutTime = DateTime.Now.AddHours(-1) },
                new Visit { Id = 2, UserId = "2", PlaceId = 2, CheckInTime = DateTime.Now.AddHours(-3), CheckOutTime = DateTime.Now.AddHours(-2) },
                new Visit { Id = 3, UserId = "3", PlaceId = 3, CheckInTime = DateTime.Now.AddHours(-1) }
            };

            _context.Visits.AddRange(visits);

            _context.SaveChanges();
        }
    }
}
