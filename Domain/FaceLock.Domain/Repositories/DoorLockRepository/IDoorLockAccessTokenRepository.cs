using FaceLock.Domain.Entities.DoorLockAggregate;

namespace FaceLock.Domain.Repositories.DoorLockRepository
{
    public interface IDoorLockAccessTokenRepository : IRepository<DoorLockAccessToken>
    {
        Task<List<DoorLockAccessToken>> GetAccessTokenByDoorLockIdAsync(string doorLockId);
    }
}
