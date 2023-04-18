using FaceLock.Domain.Entities.UserAggregate;

namespace FaceLock.Domain.Entities.DoorLockAggregate
{
    /// <summary>
    /// Сlass represents a door lock history.
    /// </summary>
    public class DoorLockHistory
    {
        public int Id { get; set; }
        public int DoorLockId { get; set; }
        public string? UserId { get; set; }
        public DateTime OpenedDateTime { get; set; }

        public DoorLock? DoorLock { get; set; }
        public User? User { get; set; }
    }
}
