using FaceLock.Domain.Entities.PlaceAggregate;
using FaceLock.Domain.Repositories;
using FaceLock.Domain.Repositories.PlaceRepository;
using FaceLock.Domain.Repositories.UserRepository;
using FaceLock.EF.Repositories;
using FaceLock.EF.Repositories.PlaceRepository;
using FaceLock.EF.Repositories.UserRepository;
using FaceLock.EF.Tests.FaceLockDBTests;
using Microsoft.EntityFrameworkCore;


namespace FaceLock.EF.Tests.RepositoriesTests
{
    [TestFixture]
    public class VisitRepositoryTests : FaceLockDBTestBase
    {
        private IUnitOfWork _unitOfWork;

        [OneTimeSetUp]
        public void SetUp()
        {
            _unitOfWork = new UnitOfWork(_context, visitRepository: new Lazy<IVisitRepository>(() => new VisitRepository(_context)));
        }

        [Test]
        public async Task GetByIdAsync_WhenVisitExists_ReturnsVisit()
        {
            // Arrange
            int visitId = 2;

            // Act
            var visit = await _unitOfWork.VisitRepository.GetByIdAsync(visitId);

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
            var visit = await _unitOfWork.VisitRepository.GetByIdAsync(visitId);

            // Assert
            Assert.IsNull(visit);
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllVisits()
        {
            // Arrange
            var expectedVisits = await _context.Visits.ToListAsync();

            // Act
            var actualVisits = await _unitOfWork.VisitRepository.GetAllAsync();

            // Assert
            Assert.AreEqual(expectedVisits.Count, actualVisits.Count());
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
                PlaceId = roomId,
                CheckInTime = checkInTime,
                CheckOutTime = checkOutTime
            };

            // Act
            await _unitOfWork.VisitRepository.AddAsync(visit);
            await _unitOfWork.SaveChangesAsync();
            // Assert
            var resultvisit = await _unitOfWork.VisitRepository.GetByIdAsync(visit.Id);

            Assert.AreEqual(roomId, resultvisit.PlaceId);
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
                PlaceId = 1,
                CheckInTime = DateTime.Now,
            };
            await _unitOfWork.VisitRepository.AddAsync(visit);
            await _unitOfWork.SaveChangesAsync();

            // Act
            visit.CheckOutTime = DateTime.Now;
            await _unitOfWork.VisitRepository.UpdateAsync(visit);
            await _unitOfWork.SaveChangesAsync();

            // Assert
            var result = await _unitOfWork.VisitRepository.GetByIdAsync(visit.Id);
            Assert.AreEqual(visit.CheckOutTime, result.CheckOutTime);
        }

        [Test]
        public async Task DeleteAsync_DeletesVisitFromDatabase()
        {
            // Arrange
            var visit = new Visit
            {
                UserId = "testUserId",
                PlaceId = 10,
                CheckInTime = DateTime.Now
            };

            await _context.Visits.AddAsync(visit);
            await _context.SaveChangesAsync();

            // Act
            await _unitOfWork.VisitRepository.DeleteAsync(visit);
            await _unitOfWork.SaveChangesAsync();

            // Assert
            var deletedVisit = await _unitOfWork.VisitRepository.GetByIdAsync(visit.Id);
            //var deletedVisit = await _context.Visits.FindAsync(visit.Id);
            Assert.IsNull(deletedVisit);
        }

        [Test]
        public async Task GetVisitsByUserIdAsync_WithValidUserId_ReturnsListOfVisits()
        {
            // Arrange
            var userId = "validUserId";
            var visit1 = new Visit { UserId = userId, PlaceId = 1, CheckInTime = DateTime.Now };
            var visit2 = new Visit { UserId = userId, PlaceId = 1, CheckInTime = DateTime.Now.AddHours(-1) };
            var visit3 = new Visit { UserId = "otherUserId", PlaceId = 1, CheckInTime = DateTime.Now };
            _context.Visits.AddRange(visit1, visit2, visit3);
            await _context.SaveChangesAsync();

            // Act
            var result = await _unitOfWork.VisitRepository.GetVisitsByUserIdAsync(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<Visit>>(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(userId, result.ToList()[0].UserId);
            Assert.AreEqual(userId, result.ToList()[1].UserId);
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
            var result = await _unitOfWork.VisitRepository.GetVisitsByUserIdAsync(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<Visit>>(result);
            Assert.AreEqual(0, result.Count());
        }

    }
}
