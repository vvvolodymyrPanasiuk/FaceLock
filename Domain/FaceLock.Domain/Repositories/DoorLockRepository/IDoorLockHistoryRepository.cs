using FaceLock.Domain.Entities.DoorLockAggregate;

namespace FaceLock.Domain.Repositories.DoorLockRepository
{
    public interface IDoorLockHistoryRepository : IRepository<DoorLockHistory>
    {
        Task<List<DoorLockHistory>> GetVisitsByUserIdAsync(string userId);
        Task<List<DoorLockHistory>> GetVisitsByDoorLockIdAsync(int doorLockId);
    }
}
