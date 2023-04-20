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

        public async Task<IEnumerable<UserDoorLockAccess>> GetAccessByDoorLockIdAsync(int doorLockId)
        {
            return await _dbSet.Where(v => v.DoorLockId == doorLockId).ToListAsync();
        }

        public async Task<IEnumerable<UserDoorLockAccess>> GetAccessByUserIdAsync(string userId)
        {
            return await _dbSet.Where(v => v.UserId == userId).ToListAsync();
        }
    }
}
