using FaceLock.Domain.Entities.UserAggregate;

namespace FaceLock.Domain.Entities.DoorLockAggregate
{
    /// <summary>
    /// Сlass represents a user's access to a door lock.
    /// </summary>
    public class UserDoorLockAccess
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int DoorLockId { get; set; }
        public bool HasAccess { get; set; }

        public User? User { get; set; }
        public DoorLock? DoorLock { get; set; }
    }
}
