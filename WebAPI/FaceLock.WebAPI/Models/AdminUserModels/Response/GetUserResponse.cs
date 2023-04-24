using System.ComponentModel.DataAnnotations;

namespace FaceLock.WebAPI.Models.AdminUserModels.Response
{
    public class GetUserResponse
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Status { get; set; }

        public GetUserResponse(
            string id, 
            string username, 
            string email, 
            string firstname, 
            string lastname,
            string status) 
        {
            Id = id;
            Username = username;
            Email = email;
            FirstName = firstname;
            LastName = lastname;
            Status = status;
        }
    }
}
