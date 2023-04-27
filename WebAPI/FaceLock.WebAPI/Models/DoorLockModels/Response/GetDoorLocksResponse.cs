using System.Collections.Generic;

namespace FaceLock.WebAPI.Models.DoorLockModels.Response
{
    public class GetDoorLocksResponse
    {
        public IEnumerable<GetDoorLockResponse> DoorLocks { get; set; }

        public GetDoorLocksResponse(IEnumerable<GetDoorLockResponse> doorLocks) 
        { 
            DoorLocks = doorLocks;
        }
    }
}
