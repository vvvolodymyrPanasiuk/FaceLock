using System.Collections.Generic;

namespace FaceLock.WebAPI.Models.UserModels.Response
{
    /// <summary>
    /// Response object containing user access information.
    /// </summary>
    public class GetUserAccessesResponse
    {
        /// <summary>
        /// List of user access records.
        /// </summary>
        /// <remarks>
        /// A list of user access records for a particular door lock.
        /// </remarks>
        /// <example>
        /// [
        ///   {
        ///     "id": 1,
        ///     "userId": "1234",
        ///     "doorLockId": 1,
        ///     "hasAccess": true
        ///   },
        ///   {
        ///     "id": 2,
        ///     "userId": "5678",
        ///     "doorLockId": 1,
        ///     "hasAccess": false
        ///   }
        /// ]
        /// </example>
        public IEnumerable<UserAccess> UserAccesses { get; set; }

        public GetUserAccessesResponse(IEnumerable<UserAccess> userAccesses) 
        {
            UserAccesses = userAccesses;
        }
    }

    /// <summary>
    /// Represents a user access record.
    /// </summary>
    public class UserAccess
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int DoorLockId { get; set; }
        public bool HasAccess { get; set; }

        public UserAccess(int id, string userId, int doorLockId, bool hasAccess)
        {
            Id = id;
            UserId = userId;
            DoorLockId = doorLockId;
            HasAccess = hasAccess;
        }
    }
}
