using System.Collections.Generic;

namespace FaceLock.WebAPI.Models.AdminUserModels.Response
{
    public class GetUserPhotosInfoResponse
    {
        public int Id { get; set; }
        public string ImageMimeType { get; set; }
        public string UserId { get; set; }

        public GetUserPhotosInfoResponse(int id, string imageMineType, string userId)
        {
            Id = id;
            ImageMimeType = imageMineType;
            UserId = userId;
        }
    }
}
