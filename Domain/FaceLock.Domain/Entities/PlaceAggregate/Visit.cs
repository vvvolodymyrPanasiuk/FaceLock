using FaceLock.Domain.Entities.UserAggregate;


namespace FaceLock.Domain.Entities.PlaceAggregate
{
    /// <summary>
    /// Kлас, що представляє візит користувача до кімнати
    /// </summary>
    public class Visit
    {
        public int Id { get; set; }

        // UserId - ідентифікатор користувача, який здійснив візит
        public string? UserId { get; set; }

        // RoomId - ідентифікатор кімнати, яку відвідав користувач
        public int RoomId { get; set; }

        // CheckInTime - час початку візиту
        public DateTime CheckInTime { get; set; }

        // CheckOutTime - час закінчення візиту (може бути null, якщо користувач ще не закінчив візит)
        public DateTime? CheckOutTime { get; set; }


        // User - посилання на користувача, який здійснив візит
        public User? User { get; set; }

        // Room - посилання на кімнату, яку відвідав користувач
        public Place? Room { get; set; }
    }
}
