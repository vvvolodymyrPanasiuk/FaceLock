using FaceLock.Domain.Entities.DoorLockAggregate;

namespace FaceLock.Domain.Repositories.DoorLockRepository
{
    /// <summary>
    /// Interface inherited from IRepository for working with the DoorLockHistory table in the database
    /// </summary>
    public interface IDoorLockHistoryRepository : IRepository<DoorLockHistory>
    {
        ///  <summary> 
        /// A method that retrieves the history of unlocking the door lock by user ID.
        ///  </summary> 
        ///  <param name="userId">User id</param> 
        ///  <returns>List of DoorLockHistory entities from the database</returns> 
        Task<IEnumerable<DoorLockHistory>> GetDoorLockHistoryByUserIdAsync(string userId);
        ///  <summary> 
        /// A method that retrieves the history of unlocking the door lock by door lock ID.
        ///  </summary> 
        ///  <param name="doorLockId">Door lock id</param> 
        ///  <returns>List of DoorLockHistory entities from the database</returns> 
        Task<IEnumerable<DoorLockHistory>> GetDoorLockHistoryByDoorLockIdAsync(int doorLockId);
    }
}
