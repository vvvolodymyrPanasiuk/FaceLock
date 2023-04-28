using System;
using System.Collections.Generic;

namespace FaceLock.WebAPI.Models.DoorLockModels.Response
{
    /// <summary>
    /// Response object containing door lock history.
    /// </summary>
    public class GetDoorLockHistoryResponse
    {
        /// <summary>
        /// Door lock histories.
        /// </summary>
        /// <remarks>
        /// The list of door lock histories associated with the user.
        /// </remarks>
        /// <example>
        /// {
        ///     "doorLockHistories": [
        ///         {
        ///             "id": 1,
        ///             "userId": "123456",
        ///             "doorLockId": 789,
        ///             "openedDateTime": "2023-04-28T09:00:00Z"
        ///         },
        ///         {
        ///             "id": 2,
        ///             "userId": "123456",
        ///             "doorLockId": 456,
        ///             "openedDateTime": "2023-04-27T10:00:00Z"
        ///         }
        ///     ]
        /// }
        /// </example>
        public IEnumerable<HistoryDoorLock> DoorLockHistories { get; set; }

        public GetDoorLockHistoryResponse(IEnumerable<HistoryDoorLock> doorLockHistories)
        {
            DoorLockHistories = doorLockHistories;
        }
    }

    /// <summary>
    /// Door lock history object.
    /// </summary>
    public class HistoryDoorLock
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int DoorLockId { get; set; }
        public DateTime OpenedDateTime { get; set; }

        public HistoryDoorLock(int id, string userId, int doorLockId, DateTime openedDateTime)
        {
            Id = id;
            UserId = userId;
            DoorLockId = doorLockId;
            OpenedDateTime = openedDateTime;
        }
    }
}
