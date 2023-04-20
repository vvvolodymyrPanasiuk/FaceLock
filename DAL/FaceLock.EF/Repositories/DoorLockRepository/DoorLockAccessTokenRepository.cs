using FaceLock.Domain.Entities.DoorLockAggregate;
using FaceLock.Domain.Repositories.DoorLockRepository;

namespace FaceLock.EF.Repositories.DoorLockRepository
{
    public class DoorLockAccessTokenRepository : Repository<DoorLockAccessToken>, IDoorLockAccessTokenRepository
    {
        public DoorLockAccessTokenRepository(FaceLockDbContext context) : base(context)
        {
        }

        public Task<List<DoorLockAccessToken>> GetAccessTokenByDoorLockIdAsync(string doorLockId)
        {
            throw new NotImplementedException();
        }
    }
}
