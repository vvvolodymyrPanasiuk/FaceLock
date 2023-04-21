using FaceLock.Domain.Entities.DoorLockAggregate;
using FaceLock.Domain.Repositories.DoorLockRepository;
using Microsoft.EntityFrameworkCore;

namespace FaceLock.EF.Repositories.DoorLockRepository
{
    public class DoorLockAccessTokenRepository : Repository<DoorLockAccessToken>, IDoorLockAccessTokenRepository
    {
        public DoorLockAccessTokenRepository(FaceLockDbContext context) : base(context)
        {
        }

        public async Task<IQueryable<DoorLockAccessToken>> GetAccessTokenByDoorLockIdAsync(int doorLockId)
        {
            return _dbSet.Where(v => v.DoorLockId == doorLockId);
        }
    }
}
