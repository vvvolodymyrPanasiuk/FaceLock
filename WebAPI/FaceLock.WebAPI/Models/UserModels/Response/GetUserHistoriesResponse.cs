using System;
using System.Collections.Generic;

namespace FaceLock.WebAPI.Models.UserModels.Response
{
    public class GetUserHistoriesResponse
    {
        public IEnumerable<UserHistory> UserHistoriesDoorLock { get; set; }

        public GetUserHistoriesResponse(IEnumerable<UserHistory> userHistoriesDoorLock)
        {
            UserHistoriesDoorLock = userHistoriesDoorLock;
        }
    }
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
