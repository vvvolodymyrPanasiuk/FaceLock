using System;
using System.Collections.Generic;

namespace FaceLock.WebAPI.Models.UserModels.Response
{
    /// <summary>
    /// Response object for getting user histories.
    /// </summary>
    public class GetUserHistoriesResponse
    {
        /// <summary>
        /// List of user history for a particular door lock.
        /// </summary>
        /// <remarks>
        /// List of <see cref="UserHistory"/> objects that contains information about door lock history.
        /// </remarks>
        /// <example>
        /// [{
        ///     "id": 1,
        ///     "userId": "3d9c7345-5c5e-4ecf-9be7-15eab1c08681",
        ///     "doorLockId": 2,
        ///     "openedDateTime": "2022-04-25T14:30:00"
        /// }]
        /// </example>
        public IEnumerable<UserHistory> UserHistoriesDoorLock { get; set; }

        public GetUserHistoriesResponse(IEnumerable<UserHistory> userHistoriesDoorLock)
        {
            UserHistoriesDoorLock = userHistoriesDoorLock;
        }
    }

    /// <summary>
    /// User history object.
    /// </summary>
    public class UserHistory
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int DoorLockId { get; set; }
        public DateTime OpenedDateTime { get; set; }

        public UserHistory(int id, string userId, int doorLockId, DateTime openedDateTime)
        {
            Id = id;
            UserId = userId;
            DoorLockId = doorLockId;
            OpenedDateTime = openedDateTime;
        }
    }
}
