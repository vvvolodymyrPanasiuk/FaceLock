using FaceLock.Domain.Entities.DoorLockAggregate;

namespace FaceLock.Domain.Repositories.DoorLockRepository
{
    /// <summary>
    /// Interface inherited from IRepository<TEntity> for working with the UserDoorLockAccess table in the database
    /// </summary>
    public interface IDoorLockAccessRepository : IRepository<UserDoorLockAccess>
    {
        ///  <summary> 
        /// A method that retrieves the user access list to door lock by the given user ID.
        ///  </summary> 
        ///  <param name="userId">User id</param> 
        ///  <returns>List<UserDoorLockAccess></returns> 
        Task<List<UserDoorLockAccess>> GetAccessByUserIdAsync(string userId);
        ///  <summary> 
        /// A method that retrieves the user access list to door lock by the given door lock ID.
        ///  </summary> 
        ///  <param name="doorLockId">Door lock id</param> 
        ///  <returns>List<UserDoorLockAccess></returns> 
        Task<List<UserDoorLockAccess>> GetAccessByDoorLockIdAsync(string doorLockId);
    }
}
