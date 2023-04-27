using System;
using System.Collections.Generic;

namespace FaceLock.WebAPI.Models.DoorLockModels.Response
{
    public class GetDoorLockHistoryResponse
    {
        public IEnumerable<HistoryDoorLock> DoorLockHistories { get; set; }

        public GetDoorLockHistoryResponse(IEnumerable<HistoryDoorLock> doorLockHistories)
        {
            DoorLockHistories = doorLockHistories;
        }
    }

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
