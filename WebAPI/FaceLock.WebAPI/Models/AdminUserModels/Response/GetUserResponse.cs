using System.ComponentModel.DataAnnotations;

namespace FaceLock.WebAPI.Models.AdminUserModels.Response
{
    /// <summary>
    /// Response object containing information about user.
    /// </summary>
    public class GetUserResponse
    {
        /// <summary>
        /// The ID of the user.
        /// </summary>
        /// <example>12345</example>
        public string Id { get; set; }

        /// <summary>
        /// The username of the user.
        /// </summary>
        /// <example>johndoe</example>
        public string Username { get; set; }

        /// <summary>
        /// The email address of the user.
        /// </summary>
        /// <example>johndoe@example.com</example>
        public string Email { get; set; }

        /// <summary>
        /// The first name of the user.
        /// </summary>
        /// <example>John</example>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the user.
        /// </summary>
        /// <example>Doe</example>
        public string LastName { get; set; }

        /// <summary>
        /// The status of the user.
        /// </summary>
        /// <example>User</example>
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
