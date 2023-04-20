using FaceLock.Domain.Entities.DoorLockAggregate;

namespace FaceLock.Domain.Repositories.DoorLockRepository
{
    /// <summary>
    /// Interface inherited from IRepository<TEntity> for working with the DoorLock table in the database
    /// </summary>
    public interface IDoorLockRepository : IRepository<DoorLock>
    {
    }
}
