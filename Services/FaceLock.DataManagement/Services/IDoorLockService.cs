using FaceLock.Domain.Entities.DoorLockAggregate;

namespace FaceLock.DataManagement.Services
{
    /// <summary>
    /// Interface to manage door lock data that interacts with the door lock aggregate repositories through the unit of work.
    /// </summary>
    public interface IDoorLockService
    {
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
        /// Gets all the access tokens for a door lock by door lock ID.
        /// </summary>
        /// <param name="doorLockId">The ID of the door lock.</param>
        /// <returns>An IEnumerable of DoorLockAccessToken objects representing the access tokens for the door lock.</returns>
        Task<IEnumerable<DoorLockAccessToken>> GetAccessTokenByDoorLockIdAsync(int doorLockId);
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
        /// Adds a new door lock.
        /// </summary>
        /// <param name="doorLock">The DoorLock object representing the new door lock.</param>
        Task AddDoorLockAsync(DoorLock doorLock);
        /// <summary>
        /// Updates a door lock.
        /// </summary>
        /// <param name="doorLock">The DoorLock object representing the updated door lock.</param>
        Task UpdateDoorLockAsync(DoorLock doorLock);
        /// <summary>
        /// Deletes a door lock.
        /// </summary>
        /// <param name="doorLock">The DoorLock object representing the door lock to be deleted.</param>
        Task DeleteDoorLockAsync(DoorLock doorLock);
        /// <summary>
        /// Adds a new door lock access for user.
        /// </summary>
        /// <param name="userDoorLockAccess">The UserDoorLockAccess object representing the new door lock access.</param>
        Task AddDoorLockAccessAsync(UserDoorLockAccess userDoorLockAccess);
        /// <summary>
        /// Updates a door lock access for user.
        /// </summary>
        /// <param name="userDoorLockAccess">The UserDoorLockAccess object representing the updated door lock access.</param>
        Task UpdateDoorLockAccessAsync(UserDoorLockAccess userDoorLockAccess);
        /// <summary>
        /// Deletes a door lock access for user.
        /// </summary>
        /// <param name="userDoorLockAccess">The UserDoorLockAccess object to delete.</param>
        Task DeleteDoorLockAccessAsync(UserDoorLockAccess userDoorLockAccess);
        /// <summary>
        /// Adds a door lock accesses for users.
        /// </summary>
        /// <param name="userDoorLockAccess">The UserDoorLockAccess objects to add.</param>
        Task AddDoorLockAccessesAsync(IEnumerable<UserDoorLockAccess> userDoorLockAccess);
        /// <summary>
        /// Updates a door lock accesses for users.
        /// </summary>
        /// <param name="userDoorLockAccess">The UserDoorLockAccess objects to update.</param>
        Task UpdateDoorLockAccessesAsync(IEnumerable<UserDoorLockAccess> userDoorLockAccess);
        /// <summary>
        /// Deletes a door lock accesses for users.
        /// </summary>
        /// <param name="userDoorLockAccess">The UserDoorLockAccess objects to delete.</param>
        Task DeleteDoorLockAccessesAsync(IEnumerable<UserDoorLockAccess> userDoorLockAccess);
        /// <summary>
        /// Adds a door lock history for user.
        /// </summary>
        /// <param name="doorLockHistory">The DoorLockHistory object to add.</param>
        Task AddDoorLockHistoryAsync(DoorLockHistory doorLockHistory);
        /// <summary>
        /// Updates a door lock history for user.
        /// </summary>
        /// <param name="doorLockHistory">The DoorLockHistory object to update.</param>
        Task UpdateDoorLockHistoryAsync(DoorLockHistory doorLockHistory);
        /// <summary>
        /// Deletes a door lock history for user.
        /// </summary>
        /// <param name="doorLockHistory">The DoorLockHistory object to delete.</param>
        Task DeleteDoorLockHistoryAsync(DoorLockHistory doorLockHistory);
    }
}
