using System.Collections.Generic;

namespace FaceLock.WebAPI.Models.DoorLockModels.Response
{
    public class GetAccessesDoorLockResponse
    {
        public IEnumerable<UserAccessDoorLock> AccessesDoorLock { get; set; }

        public GetAccessesDoorLockResponse(IEnumerable<UserAccessDoorLock> accessDoorLocks) 
        { 
            AccessesDoorLock = accessDoorLocks;
        }
    }

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
