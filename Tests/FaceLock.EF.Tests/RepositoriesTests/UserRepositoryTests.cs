using FaceLock.Domain.Entities.UserAggregate;
using FaceLock.Domain.Repositories;
using FaceLock.EF.Repositories;
using FaceLock.EF.Tests.FaceLockDBTests;
using Microsoft.EntityFrameworkCore;


namespace FaceLock.EF.Tests.RepositoriesTests
{
    [TestFixture]
    public class UserRepositoryTests : FaceLockDBTestBase
    {
        private IUserRepository _userRepository;

        [OneTimeSetUp]
        public void SetUp()
        {
            _userRepository = new UserRepository(_context);
        }
        
        [Test]
        public async Task GetUserByUsernameAsync_UserExists_ReturnsUser()
        {
            // Arrange
            string username = "johndoe";

            // Act
            var result = await _userRepository.GetUserByUsernameAsync(username);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(username, result.UserName);
        }

        [Test]
        public async Task GetUserByUsernameAsync_UserDoesNotExist_ReturnsNull()
        {
            // Arrange
            string username = "nonexistentuser";

            // Act
            var result = await _userRepository.GetUserByUsernameAsync(username);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var expectedUser = new User
            {
                UserName = "testuser",
                FirstName = "John",
                LastName = "Doe",
                Status = "active"
            };
            await _userRepository.AddAsync(expectedUser);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userRepository.GetByIdAsync(expectedUser.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedUser.Id, result.Id);
            Assert.AreEqual(expectedUser.UserName, result.UserName);
            Assert.AreEqual(expectedUser.FirstName, result.FirstName);
            Assert.AreEqual(expectedUser.LastName, result.LastName);
            Assert.AreEqual(expectedUser.Status, result.Status);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            string userId = "666";

            // Act
            var result = await _userRepository.GetByIdAsync(userId);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllUsers()
        {
            // Arrange
            var expectedCount = 3;

            // Act
            var users = await _userRepository.GetAllAsync();

            // Assert
            Assert.AreEqual(expectedCount, users.Count);
        }

        [Test]
        public async Task AddAsync_ShouldAddUserToDatabase()
        {
            // Arrange
            var user = new User { UserName = "TestUser", FirstName = "John", LastName = "Doe", Status = "Active" };

            // Act
            await _userRepository.AddAsync(user);

            // Assert
            var addedUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == "TestUser");
            Assert.IsNotNull(addedUser);
            Assert.AreEqual("TestUser", addedUser.UserName);
            Assert.AreEqual("John", addedUser.FirstName);
            Assert.AreEqual("Doe", addedUser.LastName);
            Assert.AreEqual("Active", addedUser.Status);

            await _userRepository.DeleteAsync(user);
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateUserToDatabase()
        {
            // Arrange
            var user = new User
            {
                FirstName = "TestUpdate",
                LastName = "TestUpdate",
                UserName = "TestUpdate",
                Email = "TestUpdate@example.com",
                Status = "active"
            };
            await _userRepository.AddAsync(user);

            // Act
            user.Status = "inactive";
            await _userRepository.UpdateAsync(user);

            // Assert
            var updatedUser = await _userRepository.GetUserByUsernameAsync(user.UserName);
            Assert.AreEqual("inactive", updatedUser.Status);
        }

        [Test]
        public async Task DeleteAsync_DeletesUserFromDatabase()
        {
            // Arrange
            var user = new User { UserName = "testuser", FirstName = "Test", LastName = "User", Status = "Active" };
            //await _context.Users.AddAsync(user);
            //await _context.SaveChangesAsync(); 
            await _userRepository.AddAsync(user);

            // Act
            await _userRepository.DeleteAsync(user);

            // Assert
            var deletedUser = await _context.Users.FindAsync(user.Id);
            Assert.IsNull(deletedUser);
        }

    }
}
