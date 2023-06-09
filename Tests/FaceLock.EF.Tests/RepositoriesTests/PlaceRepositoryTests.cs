﻿using FaceLock.Domain.Entities.PlaceAggregate;
using FaceLock.Domain.Repositories;
using FaceLock.Domain.Repositories.PlaceRepository;
using FaceLock.EF.Repositories;
using FaceLock.EF.Repositories.PlaceRepository;
using FaceLock.EF.Tests.FaceLockDBTests;
using Microsoft.EntityFrameworkCore;


namespace FaceLock.EF.Tests.RepositoriesTests
{
    [TestFixture]
    public class PlaceRepositoryTests : FaceLockDBTestBase
    {
        private IUnitOfWork _unitOfWork;

        [OneTimeSetUp]
        public void SetUp()
        {
            _unitOfWork = new UnitOfWork(_context, placeRepository: new Lazy<IPlaceRepository>(() => new PlaceRepository(_context)));
        }
        
        [Test]
        public async Task GetByIdAsync_Returns_Room_With_Correct_Id()
        {
            // Arrange
            var room = new Place { Name = "Test Room", Description = "123" };
            await _unitOfWork.PlaceRepository.AddAsync(room);

            // Act
            var result = await _unitOfWork.PlaceRepository.GetByIdAsync(room.Id);

            // Assert
            Assert.AreEqual(room.Id, result.Id);
        }

        [Test]
        public async Task GetAllRoomsAsync_ReturnsAllRooms()
        {
            // Arrange
            var expectedCount = 3;

            // Act
            var rooms = await _unitOfWork.PlaceRepository.GetAllAsync();
            _unitOfWork.SaveChangesAsync();

            // Assert
            Assert.AreEqual(expectedCount, rooms.Count());
        }

        [Test]
        public async Task AddAsync_AddsRoomToDatabase()
        {
            // Arrange
            var room = new Place { Name = "Test Room", Description = "123" };

            // Act
            await _unitOfWork.PlaceRepository.AddAsync(room);
            _unitOfWork.SaveChangesAsync();

            // Assert
            var result = await _context.Places.FirstOrDefaultAsync(r => r.Id == room.Id);
            Assert.IsNotNull(result);
            Assert.AreEqual("Test Room", result.Name);
            Assert.AreEqual("123", result.Description);

            _unitOfWork.PlaceRepository.DeleteAsync(room);
            _unitOfWork.SaveChangesAsync();
        }

        [Test]
        public async Task UpdateAsync_Should_Update_Room()
        {
            // Arrange
            var room = await _context.Places.FirstAsync();
            room.Name = "Updated Room Name";

            // Act
            await _unitOfWork.PlaceRepository.UpdateAsync(room);
            _unitOfWork.SaveChangesAsync();

            // Assert
            var updatedRoom = await _context.Places.FindAsync(room.Id);
            Assert.AreEqual(room.Name, updatedRoom.Name);
        }

        [Test]
        public async Task DeleteAsync_DeletesRoomFromDatabase()
        {
            // Arrange
            var room = new Place { Name = "Test Room", Description = "101" };
            await _context.Places.AddAsync(room);
            await _context.SaveChangesAsync();

            // Act
            await _unitOfWork.PlaceRepository.DeleteAsync(room);
            _unitOfWork.SaveChangesAsync();
            var result = await _unitOfWork.PlaceRepository.GetByIdAsync(room.Id);

            // Assert
            Assert.IsNull(result);
        }
    }
}
