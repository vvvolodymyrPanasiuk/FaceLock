using FaceLock.Domain.Entities.DoorLockAggregate;
using FaceLock.Domain.Repositories.DoorLockRepository;

namespace FaceLock.EF.MySql.Repositories.DoorLockRepository
{
    public class DoorLockRepository : Repository<DoorLock>, IDoorLockRepository
    {
        public DoorLockRepository(FaceLockMySqlDbContext context) : base(context)
        {
        }
    }
}
