using FaceLock.Domain.Entities.DoorLockAggregate;
using FaceLock.Domain.Repositories;
using FaceLock.Domain.Repositories.DoorLockRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceLock.EF.Repositories.DoorLockRepository
{
    public class DoorLockHistoryRepository : Repository<DoorLockHistory>, IDoorLockHistoryRepository
    {
        public DoorLockHistoryRepository(FaceLockDbContext context) : base(context)
        {
        }

        public Task<List<DoorLockHistory>> GetDoorLockHistoryByDoorLockIdAsync(int doorLockId)
        {
            throw new NotImplementedException();
        }

        public Task<List<DoorLockHistory>> GetDoorLockHistoryByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
