using FaceLock.Recognition.DTO;

namespace FaceLock.Recognition.Services
{
    /// <summary>
    /// Represents a face recognition service.
    /// </summary>
    /// <typeparam name="T">The type of the user ID.</typeparam>
    public interface IFaceRecognitionService<T>
    {
        /// <summary>
        /// Trains the face recognition model asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task TrainModelAsync();

        /// <summary>
        /// Recognizes a user asynchronously based on the provided face recognition request.
        /// </summary>
        /// <param name="request">The face recognition request.</param>
        /// <returns>A task representing the asynchronous operation that returns the face recognition result.</returns>
        Task<FaceRecognitionResult<T>> RecognizeUserAsync(FaceRecognitionRequest<T> request);

        /// <summary>
        /// Recognizes multiple users asynchronously based on the provided face recognition requests.
        /// </summary>
        /// <param name="requests">The face recognition requests.</param>
        /// <returns>A task representing the asynchronous operation that returns the face recognition result.</returns>
        Task<FaceRecognitionResult<T>> RecognizeUsersAsync(List<FaceRecognitionRequest<T>> requests);

        /// <summary>
        /// Adds a user face asynchronously to the face recognition model.
        /// </summary>
        /// <param name="request">The face recognition request.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddUserFaceAsync(FaceRecognitionRequest<T> request);

        /// <summary>
        /// Adds multiple user faces asynchronously to the face recognition model.
        /// </summary>
        /// <param name="requests">The face recognition requests.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddUserFacesAsync(List<FaceRecognitionRequest<T>> requests);
    }
}
