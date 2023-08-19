using FaceLock.Domain.Entities.DoorLockAggregate;
using FaceLock.Domain.Repositories.DoorLockRepository;

namespace FaceLock.EF.MySql.Repositories.DoorLockRepository
{
    public class DoorLockSecurityInfoRepository : Repository<DoorLockSecurityInfo>, IDoorLockSecurityInfoRepository
    {
        public DoorLockSecurityInfoRepository(FaceLockMySqlDbContext context) : base(context)
        {
        }
    }
}
