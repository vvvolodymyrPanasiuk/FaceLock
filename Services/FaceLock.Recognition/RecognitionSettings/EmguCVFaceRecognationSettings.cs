namespace FaceLock.Recognition.RecognitionSettings
{
    /// <summary>
    /// This class represents the settings for EmguCV face recognition project.
    /// </summary>
    public class EmguCVFaceRecognationSettings
    {
        /// <summary>
        /// Gets or sets the file path for the JSON file containing the vector of integers.
        /// </summary>
        public string vectorOfIntJsonFilePath { get; set; }

        /// <summary>
        /// Gets or sets the file path for the JSON file containing the vector of matrices.
        /// </summary>
        public string vectorOfMatJsonFilePath { get; set; }

        /// <summary>
        /// Gets or sets the file path for the cascade classifier file.
        /// </summary>
        public string cascadeClassifierFilePath { get; set; }

        /// <summary>
        /// Gets or sets the file path for the Emgu training model file.
        /// </summary>
        public string emguTrainingModelFilePath { get; set; }

        /// <summary>
        /// Gets or sets the file path for the JSON file containing the mapping of user IDs to labels.
        /// </summary>
        public string userIdToLabelMapJsonFilePath { get; set; }

        /// <summary>
        /// Gets or sets the file path for the JSON file containing the mapping of labels to user IDs.
        /// </summary>
        public string labelToUserIdMapJsonFilePath { get; set; }
    }
}