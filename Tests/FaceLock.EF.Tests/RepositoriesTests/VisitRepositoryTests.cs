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
    public class VisitRepositoryTests : FaceLockDBTestBase
    {
        private IVisitRepository _visitRepository;

        [OneTimeSetUp]
        public void SetUp()
        {
            _visitRepository = new VisitRepository(_context);
        }
        
        [Test]
        public async Task GetByIdAsync_WhenVisitExists_ReturnsVisit()
        {
            // Arrange
            int visitId = 2;

            // Act
            var visit = await _visitRepository.GetByIdAsync(visitId);

            // Assert
            Assert.IsNotNull(visit);
            Assert.That(visitId, Is.EqualTo(visit.Id));
        }

        [Test]
        public async Task GetByIdAsync_WhenVisitDoesNotExist_ReturnsNull()
        {
            // Arrange
            int visitId = 10;

            // Act
            var visit = await _visitRepository.GetByIdAsync(visitId);

            // Assert
            Assert.IsNull(visit);
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllVisits()
        {
            // Arrange
            var expectedVisits = await _context.Visits.ToListAsync();

            // Act
            var actualVisits = await _visitRepository.GetAllAsync();

            // Assert
            Assert.AreEqual(expectedVisits.Count, actualVisits.Count);
            Assert.IsTrue(expectedVisits.Select(v => v.Id).SequenceEqual(actualVisits.Select(v => v.Id)));
        }

        [Test]
        public async Task AddAsync_ShouldAddVisitToDatabase()
        {
            // Arrange
            var userId = "10";
            var roomId = 10;
            var checkInTime = DateTime.Now;
            var checkOutTime = checkInTime.AddMinutes(30);

            // Створюємо новий візит
            var visit = new Visit
            {
                UserId = userId,
                RoomId = roomId,
                CheckInTime = checkInTime,
                CheckOutTime = checkOutTime
            };

            // Act
            await _visitRepository.AddAsync(visit);

            // Assert
            var resultvisit = await _visitRepository.GetByIdAsync(visit.Id);

            Assert.AreEqual(roomId, resultvisit.RoomId);
            Assert.AreEqual(checkInTime, resultvisit.CheckInTime);
            Assert.AreEqual(checkOutTime, resultvisit.CheckOutTime);
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateVisitInDatabase()
        {
            // Arrange
            var visit = new Visit
            {
                UserId = "testuser",
                RoomId = 1,
                CheckInTime = DateTime.Now,
            };
            await _visitRepository.AddAsync(visit);

            // Act
            visit.CheckOutTime = DateTime.Now;
            await _visitRepository.UpdateAsync(visit);

            // Assert
            var result = await _visitRepository.GetByIdAsync(visit.Id);
            Assert.AreEqual(visit.CheckOutTime, result.CheckOutTime);
        }

        [Test]
        public async Task DeleteAsync_DeletesVisitFromDatabase()
        {
            // Arrange
            var visit = new Visit
            {
                UserId = "testUserId",
                RoomId = 10,
                CheckInTime = DateTime.Now
            };

            await _context.Visits.AddAsync(visit);
            await _context.SaveChangesAsync();

            // Act
            await _visitRepository.DeleteAsync(visit);

            // Assert
            var deletedVisit = await _visitRepository.GetByIdAsync(visit.Id);
            //var deletedVisit = await _context.Visits.FindAsync(visit.Id);
            Assert.IsNull(deletedVisit);
        }

        [Test]
        public async Task GetVisitsByUserIdAsync_WithValidUserId_ReturnsListOfVisits()
        {
            // Arrange
            var userId = "validUserId";
            var visit1 = new Visit { UserId = userId, RoomId = 1, CheckInTime = DateTime.Now };
            var visit2 = new Visit { UserId = userId, RoomId = 1, CheckInTime = DateTime.Now.AddHours(-1) };
            var visit3 = new Visit { UserId = "otherUserId", RoomId = 1, CheckInTime = DateTime.Now };
            _context.Visits.AddRange(visit1, visit2, visit3);
            await _context.SaveChangesAsync();

            // Act
            var result = await _visitRepository.GetVisitsByUserIdAsync(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<Visit>>(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(userId, result[0].UserId);
            Assert.AreEqual(userId, result[1].UserId);
        }

        [Test]
        public async Task GetVisitsByUserIdAsync_WithInvalidUserId_ReturnsEmptyList()
        {
            // Arrange
            var userId = "invalidUserId";
            var visit1 = new Visit { UserId = "validId", CheckInTime = DateTime.Now };
            var visit2 = new Visit { UserId = "otherUserId", CheckInTime = DateTime.Now };
            _context.Visits.AddRange(visit1, visit2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _visitRepository.GetVisitsByUserIdAsync(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<Visit>>(result);
            Assert.AreEqual(0, result.Count);
        }

    }
}
