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
        /// Gets a User entity by user email.
        /// </summary>
        /// <param name="userEmail">The user email of the User entity to retrieve.</param>
        /// <returns>The User entity with the user email.</returns>
        Task<User> GetUserByEmailAsync(string userEmail);
        /// <summary>
        /// Gets a boolen that User entity is exist by user email.
        /// </summary>
        /// <param name="userEmail">The user email of the User entity to retrieve.</param>
        /// <returns>The true if user exist with the user email.</returns>
        Task<bool> IsExistUserByEmailAsync(string userEmail);
        /// <summary>
        /// Gets all User entities for a User entity.
        /// </summary>
        /// <param name="usersId">The list ID of the User entity to retrieve list of User entities.</param>
        /// <returns>An IEnumerable of User entities.</returns>
        Task<IEnumerable<User>> GetUsersByIdAsync(IEnumerable<string> usersId);
        /// <summary>
        /// Gets all UserFace entities for a User entity.
        /// </summary>
        /// <param name="userId">The ID of the User entity to retrieve UserFace entities for.</param>
        /// <returns>An IEnumerable of UserFace entities for the specified User entity.</returns>
        Task<IEnumerable<UserFace>> GetAllUserFacesAsync(string userId);
        /// <summary>
        /// Gets a UserFace entity by user ID and face ID.
        /// </summary>
        /// <param name="userId">The ID of the User entity to retrieve.</param>
        /// <param name="faceId">The ID of the UserFace entity to retrieve.</param>
        /// <returns>The UserFace entity with the specified ID.</returns>
        Task<UserFace> GetUserFaceByIdAsync(string userId, int faceId);
        /// <summary>
        /// Gets a UserFace entity by user ID and faces ID.
        /// </summary>
        /// <param name="userId">The ID of the User entity to retrieve.</param>
        /// <param name="facesId">The list ID of the UserFace entity to retrieve.</param>
        /// <returns>An IEnumerable of UserFace entities with the specified ID.</returns>
        Task<IEnumerable<UserFace>> GetUserFacesByIdAsync(string userId, IEnumerable<int> facesId);
        /// <summary>
        /// Gets all User entities for a User entity.
        /// </summary>
        /// <returns>An IEnumerable of User entities.</returns>
        Task<IEnumerable<User>> GetAllUsersAsync();
    }
}
