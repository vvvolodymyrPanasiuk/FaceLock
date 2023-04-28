using System.Collections.Generic;

namespace FaceLock.WebAPI.Models.DoorLockModels.Response
{
    /// <summary>
    /// Response object for getting a user's accesses to door locks.
    /// </summary>
    public class GetAccessesDoorLockResponse
    {
        /// <summary>
        /// The user's accesses to door locks.
        /// </summary>
        /// <remarks>
        /// A collection of objects containing the user's ID, the ID of the door lock, and whether or not the user has access to it.
        /// </remarks>
        /// <example>
        /// {
        ///     "accessesDoorLock": [
        ///         {
        ///             "id": 1,
        ///             "userId": "123456",
        ///             "doorLockId": 1,
        ///             "hasAccess": true
        ///         },
        ///         {
        ///             "id": 2,
        ///             "userId": "123456",
        ///             "doorLockId": 2,
        ///             "hasAccess": false
        ///         }
        ///     ]
        /// }
        /// </example>
        public IEnumerable<UserAccessDoorLock> AccessesDoorLock { get; set; }

        public GetAccessesDoorLockResponse(IEnumerable<UserAccessDoorLock> accessDoorLocks) 
        { 
            AccessesDoorLock = accessDoorLocks;
        }
    }

    /// <summary>
    /// Object containing a user's access to a single door lock.
    /// </summary>
    public class UserAccessDoorLock
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int DoorLockId { get; set; }
        public bool HasAccess { get; set; }

        public UserAccessDoorLock(int id, string userId, int doorLockId, bool hasAccess)
        {
            Id = id;
            UserId = userId;
            DoorLockId = doorLockId;
            HasAccess = hasAccess;
        }
    }
}
