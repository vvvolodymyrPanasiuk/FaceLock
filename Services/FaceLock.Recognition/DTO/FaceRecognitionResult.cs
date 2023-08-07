namespace FaceLock.Recognition.DTO
{
    /// <summary>
    /// Represents the result of face recognition.
    /// </summary>
    /// <typeparam name="T">The type of the user ID.</typeparam>
    public class FaceRecognitionResult<T>
    {
        /// <summary>
        /// Gets or sets the user ID for the recognized face.
        /// </summary>
        public T? UserId { get; set; }

        /// <summary>
        /// Gets or sets the prediction distance for the recognized face.
        /// </summary>
        public double? PredictionDistance { get; set; }
    }
}
