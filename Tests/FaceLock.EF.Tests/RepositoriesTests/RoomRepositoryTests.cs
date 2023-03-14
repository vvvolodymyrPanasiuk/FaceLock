using FaceLock.Domain.Entities.RoomAggregate;
using FaceLock.Domain.Repositories;
using FaceLock.EF.Repositories;
using FaceLock.EF.Tests.FaceLockDBTests;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceLock.EF.Tests.RepositoriesTests
{
    [TestFixture]
    public class RoomRepositoryTests : FaceLockDBTestBase
    {
        private IRoomRepository _roomRepository;

        [OneTimeSetUp]
        public void SetUp()
        {
            _roomRepository = new RoomRepository(_context);
        }
        
        [Test]
        public async Task GetByIdAsync_Returns_Room_With_Correct_Id()
        {
            // Arrange
            var room = new Room { Name = "Test Room", NumberRoom = "123" };
            await _roomRepository.AddAsync(room);

            // Act
            var result = await _roomRepository.GetByIdAsync(room.Id);

            // Assert
            Assert.AreEqual(room.Id, result.Id);
        }

        [Test]
        public async Task GetAllRoomsAsync_ReturnsAllRooms()
        {
            // Arrange
            var expectedCount = 3;

            // Act
            var rooms = await _roomRepository.GetAllAsync();

            // Assert
            Assert.AreEqual(expectedCount, rooms.Count);
        }

        [Test]
        public async Task AddAsync_AddsRoomToDatabase()
        {
            // Arrange
            var room = new Room { Name = "Test Room", NumberRoom = "123" };

            // Act
            await _roomRepository.AddAsync(room);

            // Assert
            var result = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == room.Id);
            Assert.IsNotNull(result);
            Assert.AreEqual("Test Room", result.Name);
            Assert.AreEqual("123", result.NumberRoom);

            _roomRepository.DeleteAsync(room);
        }

        [Test]
        public async Task UpdateAsync_Should_Update_Room()
        {
            // Arrange
            var room = await _context.Rooms.FirstAsync();
            room.Name = "Updated Room Name";

            // Act
            await _roomRepository.UpdateAsync(room);

            // Assert
            var updatedRoom = await _context.Rooms.FindAsync(room.Id);
            Assert.AreEqual(room.Name, updatedRoom.Name);
        }

        [Test]
        public async Task DeleteAsync_DeletesRoomFromDatabase()
        {
            // Arrange
            var room = new Room { Name = "Test Room", NumberRoom = "101" };
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();

            // Act
            await _roomRepository.DeleteAsync(room);
            var result = await _roomRepository.GetByIdAsync(room.Id);

            // Assert
            Assert.IsNull(result);
        }
    }
}
