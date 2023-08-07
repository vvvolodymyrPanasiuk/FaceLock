using FaceLock.Recognition.DTO;
using Microsoft.AspNetCore.Http;

namespace FaceLock.Recognition.Services
{
    /// <summary>
    /// Represents a face recognition service.
    /// </summary>
    /// <typeparam name="T">The type of the user ID.</typeparam>
    public interface IFaceRecognitionService<T>
    {
        /// <summary>
        /// Recognizes a user by a single face image and face ID.
        /// </summary>
        /// <param name="imageData">The byte array of the image data.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the face recognition result.</returns>
        Task<FaceRecognitionResult<T>> RecognizeUserByFaceAsync(byte[] imageData);

        /// <summary>
        /// Recognizes users by multiple face images and their face IDs.
        /// </summary>
        /// <param name="imagesData">The list of byte arrays of the image data.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the face recognition result.</returns>
        Task<FaceRecognitionResult<T>> RecognizeUserByFacesAsync(List<byte[]> imagesData);

        /// <summary>
        /// Recognizes a user by a single face image and face ID.
        /// </summary>
        /// <param name="imageData">The image data as an IFormFile.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the face recognition result.</returns>
        Task<FaceRecognitionResult<T>> RecognizeUserByFaceAsync(IFormFile imageData);

        /// <summary>
        /// Recognizes users by multiple face images and their face IDs.
        /// </summary>
        /// <param name="imagesData">The collection of image data as an IFormFileCollection.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the face recognition result.</returns>
        Task<FaceRecognitionResult<T>> RecognizeUserByFacesAsync(IFormFileCollection imagesData);

        /// <summary>
        /// Adds a user face to the face recognition model asynchronously.
        /// </summary>
        /// <param name="userId">The type of the user ID.</param>
        /// <param name="ImageData">The byte array of the image data.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddUserFaceToTrainModelAsync(T userId, byte[] imageData);

        /// <summary>
        /// Adds multiple user faces to the face recognition model asynchronously.
        /// </summary>
        /// <param name="userId">The type of the user ID.</param>
        /// <param name="imagesData">The list of byte arrays of the image data.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddUserFacesToTrainModelAsync(T userId, List<byte[]> imagesData);

        /// <summary>
        /// Adds a user face to the face recognition model asynchronously.
        /// </summary>
        /// <param name="userId">The type of the user ID.</param>
        /// <param name="imageData">The image data as an IFormFile.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddUserFaceToTrainModelAsync(T userId, IFormFile imageData);

        /// <summary>
        /// Adds multiple user faces to the face recognition model asynchronously.
        /// </summary>
        /// <param name="userId">The type of the user ID.</param>
        /// <param name="imagesData">The collection of image data as an IFormFileCollection.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddUserFacesToTrainModelAsync(T userId, IFormFileCollection imagesData);
    }
}
