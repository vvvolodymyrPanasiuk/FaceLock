using FaceLock.Domain.Entities.DoorLockAggregate;
using FaceLock.Domain.Repositories.DoorLockRepository;
using Microsoft.EntityFrameworkCore;

namespace FaceLock.EF.Repositories.DoorLockRepository
{
    public class DoorLockSecurityInfoRepository : Repository<DoorLockSecurityInfo>, IDoorLockSecurityInfoRepository
    {
        public DoorLockSecurityInfoRepository(FaceLockDbContext context) : base(context)
        {
        }
    }
}
