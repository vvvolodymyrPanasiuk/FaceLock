using System.Collections.Generic;

namespace FaceLock.WebAPI.Models.AdminUserModels.Response
{
    /// <summary>
    /// Response model for getting a list of users.
    /// </summary>
    /// <remarks>
    /// Use this model to return a list of users to the client.
    /// </remarks>
    public class GetUsersResponse
    {
        /// <summary>
        /// A list of users.
        /// </summary>
        /// <example>
        /// {
        ///     "users": [
        ///         {
        ///             "id": "123",
        ///             "name": "John Doe",
        ///             "email": "johndoe@example.com",
        ///             "phone": "123-456-7890"
        ///         },
        ///         {
        ///             "id": "456",
        ///             "name": "Jane Doe",
        ///             "email": "janedoe@example.com",
        ///             "phone": "987-654-3210"
        ///         }
        ///     ]
        /// }
        /// </example>
        public IEnumerable<GetUserResponse> Users { get; set; }

        public GetUsersResponse(IEnumerable<GetUserResponse> users) 
        {
            Users = users;
        }
    }
}
