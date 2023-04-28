using System.Collections.Generic;

namespace FaceLock.WebAPI.Models.AdminUserModels.Response
{
    /// <summary>
    /// Response object containing information about user photos.
    /// </summary>
    public class GetUserPhotosInfoResponse
    {
        /// <summary>
        /// The unique identifier of the photo.
        /// </summary>
        /// <example>12345</example>
        public int Id { get; set; }

        /// <summary>
        /// The MIME type of the photo.
        /// </summary>
        /// <example>image/png</example>
        public string ImageMimeType { get; set; }

        /// <summary>
        /// The unique identifier of the user who the photo belongs to.
        /// </summary>
        /// <example>fdvdfavfdsvrs213dsav</example>
        public string UserId { get; set; }

        public GetUserPhotosInfoResponse(int id, string imageMineType, string userId)
        {
            Id = id;
            ImageMimeType = imageMineType;
            UserId = userId;
        }
    }
}
