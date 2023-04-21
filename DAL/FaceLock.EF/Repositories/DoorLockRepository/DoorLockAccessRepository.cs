using FaceLock.Domain.Entities.DoorLockAggregate;
using FaceLock.Domain.Repositories.DoorLockRepository;
using Microsoft.EntityFrameworkCore;

namespace FaceLock.EF.Repositories.DoorLockRepository
{
    public class DoorLockAccessRepository : Repository<UserDoorLockAccess>, IDoorLockAccessRepository
    {
        public DoorLockAccessRepository(FaceLockDbContext context) : base(context)
        {
        }

        public async Task<IQueryable<UserDoorLockAccess>> GetAccessByDoorLockIdAsync(int doorLockId)
        {
            return _dbSet.Where(v => v.DoorLockId == doorLockId);
        }

        public async Task<IQueryable<UserDoorLockAccess>> GetAccessByUserIdAsync(string userId)
        {
            return _dbSet.Where(v => v.UserId == userId);
        }
    }
}
