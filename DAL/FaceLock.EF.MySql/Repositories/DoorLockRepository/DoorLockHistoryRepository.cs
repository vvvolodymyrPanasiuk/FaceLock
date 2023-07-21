using FaceLock.Domain.Entities.DoorLockAggregate;
using FaceLock.Domain.Repositories.DoorLockRepository;

namespace FaceLock.EF.MySql.Repositories.DoorLockRepository
{
    public class DoorLockHistoryRepository : Repository<DoorLockHistory>, IDoorLockHistoryRepository
    {
        public DoorLockHistoryRepository(FaceLockMySqlDbContext context) : base(context)
        {
        }

        public async Task<IQueryable<DoorLockHistory>> GetDoorLockHistoryByDoorLockIdAsync(int doorLockId)
        {
            return _dbSet.Where(v => v.DoorLockId ==  doorLockId);
        }

        public async Task<IQueryable<DoorLockHistory>> GetDoorLockHistoryByUserIdAsync(string userId)
        {
            return _dbSet.Where(v => v.UserId == userId);
        }
    }
}
