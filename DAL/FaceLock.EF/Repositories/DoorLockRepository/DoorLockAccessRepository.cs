using FaceLock.Domain.Entities.DoorLockAggregate;
using FaceLock.Domain.Repositories.DoorLockRepository;

namespace FaceLock.EF.Repositories.DoorLockRepository
{
    public class DoorLockAccessRepository : Repository<UserDoorLockAccess>, IDoorLockAccessRepository
    {
        public DoorLockAccessRepository(FaceLockDbContext context) : base(context)
        {
        }

        public Task<List<UserDoorLockAccess>> GetAccessByDoorLockIdAsync(string doorLockId)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserDoorLockAccess>> GetAccessByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
