namespace FaceLock.Domain.Entities.DoorLockAggregate
{
    /// <summary>
    /// Сlass represents a door lock access token.
    /// </summary>
    public class DoorLockAccessToken
    {
        public int Id { get; set; }
        public int DoorLockId { get; set; }
        public string AccessToken { get; set; }
        public bool Utilized { get; set; }

        public DoorLock? DoorLock { get; set; }
    }
}
