using System.Collections.Generic;

namespace FaceLock.WebAPI.Models.UserModels.Response
{
    public class GetUserAccessesResponse
    {
        public IEnumerable<UserAccess> UserAccesses { get; set; }
        public GetUserAccessesResponse(IEnumerable<UserAccess> userAccesses) 
        {
            UserAccesses = userAccesses;
        }
    }

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
