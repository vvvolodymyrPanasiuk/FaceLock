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

        public async Task<List<DoorLockHistory>> GetDoorLockHistoryByDoorLockIdAsync(int doorLockId)
        {
            return await _dbSet.Where(v => v.DoorLockId ==  doorLockId).ToListAsync();
        }

        public async Task<List<DoorLockHistory>> GetDoorLockHistoryByUserIdAsync(string userId)
        {
            return await _dbSet.Where(v => v.UserId == userId).ToListAsync();
        }
    }
}
