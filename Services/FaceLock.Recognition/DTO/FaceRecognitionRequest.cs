namespace FaceLock.Recognition.DTO
{
    /// <summary>
    /// Represents a request for face recognition.
    /// </summary>
    /// <typeparam name="T">The type of the user ID.</typeparam>
    public class FaceRecognitionRequest<T>
    {
        /// <summary>
        /// Gets or sets the image data for the face recognition.
        /// </summary>
        public byte[] ImageData { get; set; }

        /// <summary>
        /// Gets or sets the user ID for the face recognition.
        /// </summary>
        public T UserId { get; set; }
    }
}
