using FaceLock.Domain.Entities.UserAggregate;
using FaceLock.Domain.Repositories.PlaceRepository;
using FaceLock.Domain.Repositories.UserRepository;
using FaceLock.EF.Repositories.PlaceRepository;
using FaceLock.EF.Repositories;
using FaceLock.EF.Repositories.UserRepository;
using FaceLock.EF.Tests.FaceLockDBTests;
using Microsoft.EntityFrameworkCore;
using FaceLock.Domain.Repositories;

namespace FaceLock.EF.Tests.RepositoriesTests
{
    [TestFixture]
    public class UserRepositoryTests : FaceLockDBTestBase
    {
        private IUnitOfWork _unitOfWork;

        [OneTimeSetUp]
        public void SetUp()
        {
            _unitOfWork = new UnitOfWork(_context, userRepository: new Lazy<IUserRepository>(() => new UserRepository(_context)));
        }

        [Test]
        public async Task GetUserByUsernameAsync_UserExists_ReturnsUser()
        {
            // Arrange
            string username = "johndoe";

            // Act
            var result = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);

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
            var result = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var expectedUser = new User
            {
                UserName = "JANEDOE232",
                FirstName = "John1132",
                LastName = "Doe1132",
                Email = "janedoe1@example.com",
                NormalizedEmail = "JANEDOE1@EXAMPLE.COM",
                NormalizedUserName = "JANEDOE232",
                PasswordHash = "jdfsafdbvfxdbfd32ufsdiovbhui",
                Status = "Active"
            };
            await _context.Users.AddAsync(expectedUser);
            await _context.SaveChangesAsync();

            // Act
            var result = await _unitOfWork.UserRepository.GetByIdAsync(expectedUser.Id);

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
            var result = await _unitOfWork.UserRepository.GetByIdAsync(userId);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllUsers()
        {
            // Arrange
            var expectedCount = 3;

            // Act
            var users = await _unitOfWork.UserRepository.GetAllAsync();

            // Assert
            Assert.AreEqual(expectedCount, users.Count());
        }

        [Test]
        public async Task AddAsync_ShouldAddUserToDatabase()
        {
            // Arrange
            var user = new User 
            { 
                UserName = "TestUser", 
                FirstName = "Jahn", 
                LastName = "Doe",
                Email = "janedoe1@example.com",
                NormalizedEmail = "JANEDOE1@EXAMPLE.COM",
                NormalizedUserName = "JANEDOE",
                PasswordHash = "jdfsafdbufsdiovbhui",
                Status = "Active"
            };

            // Act
            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            // Assert
            var addedUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == "TestUser");
            Assert.IsNotNull(addedUser);
            Assert.AreEqual("TestUser", addedUser.UserName);
            Assert.AreEqual("Jahn", addedUser.FirstName);
            Assert.AreEqual("Doe", addedUser.LastName);
            Assert.AreEqual("Active", addedUser.Status);

            await _unitOfWork.UserRepository.DeleteAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateUserToDatabase()
        {
            // Arrange
            var user = new User
            {
                UserName = "JANEDOE12334",
                FirstName = "Jahna",
                LastName = "Doe",
                Email = "janedoe12@example.com",
                NormalizedEmail = "JANEDOE12@EXAMPLE.COM",
                NormalizedUserName = "JANEDOE12334",
                PasswordHash = "jdfsafdbufsdiovbhu214i1112",
                Status = "Active"
            };
            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            // Act
            user.Status = "inactive";
            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            // Assert
            var updatedUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(user.UserName);
            Assert.AreEqual("inactive", updatedUser.Status);
        }

        [Test]
        public async Task DeleteAsync_DeletesUserFromDatabase()
        {
            // Arrange
            var user = new User
            {
                UserName = "JANEDOE112",
                FirstName = "John112",
                LastName = "Doe113",
                Email = "janedoe112@example.com",
                NormalizedEmail = "JANEDOE112@EXAMPLE.COM",
                NormalizedUserName = "JANEDOE112",
                PasswordHash = "jdfsafdbufsdiovbh213213ui",
                Status = "Active"
            };
            //await _context.Users.AddAsync(user);
            //await _context.SaveChangesAsync(); 
            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            // Act
            await _unitOfWork.UserRepository.DeleteAsync(user);
            await _unitOfWork.SaveChangesAsync();

            // Assert
            var deletedUser = await _context.Users.FindAsync(user.Id);
            Assert.IsNull(deletedUser);
        }

    }
}
