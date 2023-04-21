using FaceLock.Domain.Entities.DoorLockAggregate;
using FaceLock.Domain.Repositories.DoorLockRepository;
using Microsoft.EntityFrameworkCore;

namespace FaceLock.EF.Repositories.DoorLockRepository
{
    public class DoorLockHistoryRepository : Repository<DoorLockHistory>, IDoorLockHistoryRepository
    {
        public DoorLockHistoryRepository(FaceLockDbContext context) : base(context)
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
