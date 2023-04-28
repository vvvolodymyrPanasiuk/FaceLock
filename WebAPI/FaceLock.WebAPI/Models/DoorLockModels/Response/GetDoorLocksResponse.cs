using System.Collections.Generic;

namespace FaceLock.WebAPI.Models.DoorLockModels.Response
{
    /// <summary>
    /// Response object for getting a list of door locks.
    /// </summary>
    public class GetDoorLocksResponse
    {
        /// <summary>
        /// The list of door locks.
        /// </summary>
        /// <remarks>
        /// The door locks retrieved from the database.
        /// </remarks>
        /// <example>
        /// {
        ///     "doorLocks": [
        ///         {
        ///             "id": 1,
        ///             "Name": "Front Door",
        ///             "Description": "The main entrance door."
        ///         },
        ///         {
        ///             "id": 2,
        ///             "Name": "Back Door",
        ///             "Description": "The rear entrance door."
        ///         }
        ///     ]
        /// }
        /// </example>
        public IEnumerable<GetDoorLockResponse> DoorLocks { get; set; }

        public GetDoorLocksResponse(IEnumerable<GetDoorLockResponse> doorLocks) 
        { 
            DoorLocks = doorLocks;
        }
    }
}
