using System.Collections.Generic;

namespace FaceLock.WebAPI.Models.DoorLockModels.Response
{
    /// <summary>
    /// Response object for getting door lock tokens.
    /// </summary>
    public class GetDoorLockTokensResponse
    {
        /// <summary>
        /// List of door lock tokens.
        /// </summary>
        /// <remarks>
        /// A list of DoorLockToken objects representing the access tokens for door locks.
        /// </remarks>
        /// <example>
        /// {
        ///     "doorLockTokens": [
        ///         {
        ///             "id": 1,
        ///             "doorLockId": 1,
        ///             "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c",
        ///             "utilized": false
        ///         },
        ///         {
        ///             "id": 2,
        ///             "doorLockId": 2,
        ///             "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c",
        ///             "utilized": true
        ///         }
        ///     ]
        /// }
        /// </example>
        public IEnumerable<DoorLockToken> DoorLockTokens { get; set;}

        public GetDoorLockTokensResponse(IEnumerable<DoorLockToken> doorLockTokens) 
        {
            DoorLockTokens = doorLockTokens;
        }
    }

    /// <summary>
    /// Object representing a door lock token.
    /// </summary>
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
