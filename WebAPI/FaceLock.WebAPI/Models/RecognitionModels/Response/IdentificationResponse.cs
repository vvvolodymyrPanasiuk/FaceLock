namespace FaceLock.WebAPI.Models.RecognitionModels.Response
{
    /// <summary>
    /// Identification response object.
    /// </summary>
    public class IdentificationResponse
    {
        /// <summary>
        /// The user's ID.
        /// </summary>
        /// <remarks>
        /// Unique identifier of the user.
        /// </remarks>
        /// <example>
        /// "123456"
        /// </example>
        public string UserId { get; set; }
        /// <summary>
        /// The prediction distance by recognition.
        /// </summary>
        /// <remarks>
        /// A measure of similarity between the recognized face and the reference face.
        /// A smaller value indicates higher similarity, and a larger value indicates lower similarity.
        /// </remarks>
        /// <example>
        /// "29.41"
        /// </example>
        public double? PredictionDistance { get; set; }

        public IdentificationResponse(string userId, double? predictionDistance)
        {
            UserId = userId;
            PredictionDistance = predictionDistance;
        }
    }
}