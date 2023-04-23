using FaceLock.Domain.Entities.DoorLockAggregate;

namespace FaceLock.DataManagement.Services.Commands
{
    /// <summary>
    /// Interface to manage door lock data (Write) that interacts with the door lock aggregate repositories through the unit of work.
    /// </summary>
    public interface ICommandDoorLockService
    {
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


        /// <summary>
        /// Creates new access tokens for a door lock by door lock ID.
        /// </summary>
        /// <param name="doorLock">The DoorLock object representing the door lock for which to create the access tokens.</param>
        /// <returns>An IEnumerable of DoorLockAccessToken objects representing the newly created access tokens for the door lock.</returns>
        Task CreateAccessTokensAsync(int doorLock);
        /// <summary>
        /// Uses an unused access token for a door lock by door lock ID.
        /// </summary>
        /// <param name="doorLockId">The ID of the door lock.</param>
        /// <returns>A DoorLockAccessToken object representing the unused access token for the door lock.</returns>
        Task<DoorLockAccessToken> UseUnusedAccessTokenAsync(int doorLockId);
        /// <summary>
        /// Updates an existing access token for a door lock.
        /// </summary>
        /// <param name="accessToken">The DoorLockAccessToken object representing the access token to update.</param>
        Task UpdateAccessTokenAsync(DoorLockAccessToken accessToken);
        /// <summary>
        /// Updates multiple existing access tokens for a door lock.
        /// </summary>
        /// <param name="accessTokens">An IEnumerable of DoorLockAccessToken objects representing the access tokens to update.</param>
        Task UpdateAccessTokensAsync(IEnumerable<DoorLockAccessToken> accessTokens);
    }
}
