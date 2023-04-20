using FaceLock.Domain.Entities.DoorLockAggregate;

namespace FaceLock.Domain.Repositories.DoorLockRepository
{
    public interface IDoorLockAccessRepository : IRepository<UserDoorLockAccess>
    {
        Task<List<UserDoorLockAccess>> GetAccessByUserIdAsync(string userId);
        Task<List<UserDoorLockAccess>> GetAccessByDoorLockIdAsync(string doorLockId);
    }
}
