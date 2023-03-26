namespace FaceLock.Domain.Entities.UserAggregate
{
    /// <summary>
    /// Сlass represents an image of the user's face
    /// </summary>
    public class UserFace
    {
        public int Id { get; set; }
        public byte[]? ImageData { get; set; }
        public string? ImageMimeType { get; set; }
        public string? UserId { get; set; }

        public User? User { get; set; }
    }
}
