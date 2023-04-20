using FaceLock.Domain.Entities.DoorLockAggregate;
using FaceLock.Domain.Repositories.DoorLockRepository;
using Microsoft.EntityFrameworkCore;

namespace FaceLock.EF.Repositories.DoorLockRepository
{
    public class DoorLockRepository : Repository<DoorLock>, IDoorLockRepository
    {
        public DoorLockRepository(FaceLockDbContext context) : base(context)
        {
        }
    }
}
