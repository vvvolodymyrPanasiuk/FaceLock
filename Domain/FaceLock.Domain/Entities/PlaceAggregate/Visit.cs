using FaceLock.Domain.Entities.UserAggregate;


namespace FaceLock.Domain.Entities.PlaceAggregate
{
    /// <summary>
    /// Class representing the user's visit to the inspection place
    /// </summary>
    public class Visit
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int PlaceId { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }

        public User? User { get; set; }
        public Place? Place { get; set; }
    }
}
