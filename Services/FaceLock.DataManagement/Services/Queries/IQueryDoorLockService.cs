using FaceLock.Domain.Entities.DoorLockAggregate;

namespace FaceLock.DataManagement.Services.Queries
{
    /// <summary>
    /// Interface to manage door lock data (Read) that interacts with the door lock aggregate repositories through the unit of work.
    /// </summary>
    public interface IQueryDoorLockService
    {
        /// <summary>
        /// Gets a door lock by ID.
        /// </summary>
        /// <param name="doorLockId">The ID of the door lock.</param>
        /// <returns>A DoorLock object representing the door lock.</returns>
        Task<DoorLock> GetDoorLockByIdAsync(int doorLockId);
        /// <summary>
        /// Gets all the door locks.
        /// </summary>
        /// <returns>An IEnumerable of DoorLock objects representing all the door locks.</returns>
        Task<IEnumerable<DoorLock>> GetAllDoorLocksAsync();


        /// <summary>
        /// Gets a user access to door lock by ID.
        /// </summary>
        /// <param name="userDoorLockAccessId">The ID of the door lock.</param>
        /// <returns>A UserDoorLockAccess object representing the user access to door lock .</returns>
        Task<UserDoorLockAccess> GetUserDoorLockAccessByIdAsync(int userDoorLockAccessId);
        /// <summary>
        /// Gets a user access to door lock by IDs.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="doorLockId">The ID of the door lock.</param>
        /// <returns>A UserDoorLockAccess object representing the user access to door lock .</returns>
        Task<UserDoorLockAccess> GetUserDoorLockAccessByIdAsync(string userId, int doorLockId);
        /// <summary>
        /// Gets all the door lock accesses for a user by user ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>An IEnumerable of UserDoorLockAccess objects representing the door lock accesses for the user.</returns>
        Task<IEnumerable<UserDoorLockAccess>> GetAccessByUserIdAsync(string userId);
        /// <summary>
        /// Gets all the door lock accesses for a door lock by door lock ID.
        /// </summary>
        /// <param name="doorLockId">The ID of the door lock.</param>
        /// <returns>An IEnumerable of UserDoorLockAccess objects representing the door lock accesses for the door lock.</returns>
        Task<IEnumerable<UserDoorLockAccess>> GetAccessByDoorLockIdAsync(int doorLockId);


        /// <summary>
        /// Gets a info about one history of door lock by ID.
        /// </summary>
        /// <param name="doorLockHistoryId">The ID of the door lock.</param>
        /// <returns>A DoorLockHistory object representing the door lock history.</returns>
        Task<DoorLockHistory> GetDoorLockHistoryByIdAsync(int doorLockHistoryId);
        /// <summary>
        /// Gets all the door lock history records for a user by user ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>An IEnumerable of DoorLockHistory objects representing the door lock history records for the user.</returns>
        Task<IEnumerable<DoorLockHistory>> GetDoorLockHistoryByUserIdAsync(string userId);
        /// <summary>
        /// Gets all the door lock history records for a door lock by door lock ID.
        /// </summary>
        /// <param name="doorLockId">The ID of the door lock.</param>
        /// <returns>An IEnumerable of DoorLockHistory objects representing the door lock history records for the door lock.</returns>
        Task<IEnumerable<DoorLockHistory>> GetDoorLockHistoryByDoorLockIdAsync(int doorLockId);


        /// <summary>
        /// Gets security information for a door lock by door lock ID.
        /// </summary>
        /// <param name="doorLockId">The ID of the door lock.</param>
        /// <returns>A DoorLockSecurityInfo object representing the security information for the door lock.</returns>
        Task<DoorLockSecurityInfo> GetSecurityInfoByDoorLockIdAsync(int doorLockId);    
    }
}
