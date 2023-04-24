using System.Collections.Generic;

namespace FaceLock.WebAPI.Models.AdminUserModels.Response
{
    public class GetUsersResponse
    {
        public IEnumerable<GetUserResponse> Users { get; set; }

        public GetUsersResponse(IEnumerable<GetUserResponse> users) 
        {
            Users = users;
        }
    }
}
