using FaceLock.Domain.Entities.DoorLockAggregate;

namespace FaceLock.Domain.Repositories.DoorLockRepository
{
    /// <summary>
    /// Interface inherited from IRepository for working with the DoorLockSecurityInfo table in the database
    /// </summary>
    public interface IDoorLockSecurityInfoRepository : IRepository<DoorLockSecurityInfo>
    {
    }
}
