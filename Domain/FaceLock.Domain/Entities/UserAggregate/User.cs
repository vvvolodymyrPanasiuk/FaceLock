using FaceLock.Domain.Entities.DoorLockAggregate;
using FaceLock.Domain.Entities.PlaceAggregate;
using Microsoft.AspNetCore.Identity;


namespace FaceLock.Domain.Entities.UserAggregate
{
    /// <summary>
    /// Class represents a user and inherits from IdentityUser
    /// </summary>
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Status { get; set; }

        public List<UserFace>? UserFaces { get; set; }  
        public List<Visit>? Visits { get; set; }
        public List<UserDoorLockAccess>? DoorLockAccesses { get; set; }
        public List<DoorLockHistory>? DoorLockHistories { get; set; }
    }
}
