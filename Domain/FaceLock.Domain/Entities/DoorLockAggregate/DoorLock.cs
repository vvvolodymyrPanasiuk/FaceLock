namespace FaceLock.Domain.Entities.DoorLockAggregate
{
    /// <summary>
    /// Сlass represents a door lock.
    /// </summary>
    public class DoorLock
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public List<UserDoorLockAccess>? DoorLockAccesses { get; set; }
        public List<DoorLockHistory>? DoorLockHistories { get; set; }
        public List<DoorLockSecurityInfo>? DoorLockAccessTokens { get; set; }
    }
}
