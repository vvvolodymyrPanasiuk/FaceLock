using System.Collections.Generic;

namespace FaceLock.WebAPI.Models.DoorLockModels.Response
{
    public class GetDoorLockTokensResponse
    {
        public IEnumerable<DoorLockToken> DoorLockTokens { get; set;}

        public GetDoorLockTokensResponse(IEnumerable<DoorLockToken> doorLockTokens) 
        {
            DoorLockTokens = doorLockTokens;
        }
    }

    public class DoorLockToken
    {
        public int Id { get; set; }
        public int DoorLockId { get; set; }
        public string AccessToken { get; set; }
        public bool Utilized { get; set; }

        public DoorLockToken(int id, int doorLockId, string accessToken, bool utilized)
        {
            Id = id;
            DoorLockId = doorLockId;
            AccessToken = accessToken;
            Utilized = utilized;
        }
    }
}
