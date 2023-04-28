namespace FaceLock.WebAPI.Models.UserModels.Response
{
    /// <summary>
    /// Response object containing user information.
    /// </summary>
    public class GetUserInfoResponse
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
        public string Id { get; set; }

        /// <summary>
        /// The user's username.
        /// </summary>
        /// <remarks>
        /// The username of the user.
        /// </remarks>
        /// <example>
        /// "johndoe"
        /// </example>
        public string Username { get; set; }

        /// <summary>
        /// The user's email address.
        /// </summary>
        /// <remarks>
        /// The email address of the user.
        /// </remarks>
        /// <example>
        /// "johndoe@example.com"
        /// </example>
        public string Email { get; set; }

        /// <summary>
        /// The user's first name.
        /// </summary>
        /// <remarks>
        /// The first name of the user.
        /// </remarks>
        /// <example>
        /// "John"
        /// </example>
        public string FirstName { get; set; }

        /// <summary>
        /// The user's last name.
        /// </summary>
        /// <remarks>
        /// The last name of the user.
        /// </remarks>
        /// <example>
        /// "Doe"
        /// </example>
        public string LastName { get; set; }

        /// <summary>
        /// The user's status.
        /// </summary>
        /// <remarks>
        /// The status of the user (e.g. "active", "inactive").
        /// </remarks>
        /// <example>
        /// "User"
        /// </example>
        public string Status { get; set; }

        public GetUserInfoResponse(
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
