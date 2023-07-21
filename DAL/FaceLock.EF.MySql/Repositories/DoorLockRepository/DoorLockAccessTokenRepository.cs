using FaceLock.Domain.Entities.DoorLockAggregate;
using FaceLock.Domain.Repositories.DoorLockRepository;

namespace FaceLock.EF.MySql.Repositories.DoorLockRepository
{
    public class DoorLockAccessTokenRepository : Repository<DoorLockAccessToken>, IDoorLockAccessTokenRepository
    {
        public DoorLockAccessTokenRepository(FaceLockMySqlDbContext context) : base(context)
        {
        }

        public async Task<IQueryable<DoorLockAccessToken>> GetAccessTokenByDoorLockIdAsync(int doorLockId)
        {
            return _dbSet.Where(v => v.DoorLockId == doorLockId);
        }
    }
}
