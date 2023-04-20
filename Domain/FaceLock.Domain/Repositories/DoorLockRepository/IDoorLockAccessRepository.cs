using FaceLock.Domain.Entities.DoorLockAggregate;

namespace FaceLock.Domain.Repositories.DoorLockRepository
{
    /// <summary>
    /// Interface inherited from IRepository for working with the UserDoorLockAccess table in the database
    /// </summary>
    public interface IDoorLockAccessRepository : IRepository<UserDoorLockAccess>
    {
        ///  <summary> 
        /// A method that retrieves the user access list to door lock by the given user ID.
        ///  </summary> 
        ///  <param name="userId">User id</param> 
        ///  <returns>List of UserDoorLockAccess entities from the database</returns> 
        Task<IEnumerable<UserDoorLockAccess>> GetAccessByUserIdAsync(string userId);
        ///  <summary> 
        /// A method that retrieves the user access list to door lock by the given door lock ID.
        ///  </summary> 
        ///  <param name="doorLockId">Door lock id</param> 
        ///  <returns>List of UserDoorLockAccess entities from the database</returns> 
        Task<IEnumerable<UserDoorLockAccess>> GetAccessByDoorLockIdAsync(int doorLockId);
    }
}
