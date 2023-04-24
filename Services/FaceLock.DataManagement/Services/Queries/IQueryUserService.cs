using FaceLock.Domain.Entities.UserAggregate;

namespace FaceLock.DataManagement.Services.Queries
{
    /// <summary>
    /// Interface to manage user data (Read) that interacts with the user aggregate repositories through the unit of work.
    /// </summary>
    public interface IQueryUserService
    {
        /// <summary>
        /// Gets a User entity by user ID.
        /// </summary>
        /// <param name="userId">The ID of the User entity to retrieve.</param>
        /// <returns>The User entity with the specified ID.</returns>
        Task<User> GetUserByIdAsync(string userId);
        /// <summary>
        /// Gets a User entity by user username.
        /// </summary>
        /// <param name="userName">The username of the User entity to retrieve.</param>
        /// <returns>The User entity with the specified username.</returns>
        Task<User> GetUserByUsernameAsync(string userName);
        /// <summary>
        /// Gets all UserFace entities for a User entity.
        /// </summary>
        /// <param name="userId">The ID of the User entity to retrieve UserFace entities for.</param>
        /// <returns>An IEnumerable of UserFace entities for the specified User entity.</returns>
        Task<IEnumerable<UserFace>> GetAllUserFacesAsync(string userId);
        /// <summary>
        /// Gets all User entities for a User entity.
        /// </summary>
        /// <returns>An IEnumerable of User entities.</returns>
        Task<IEnumerable<User>> GetAllUsersAsync();
    }
}
