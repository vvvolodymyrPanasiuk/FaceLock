using FaceLock.Domain.Entities.UserAggregate;

namespace FaceLock.DataManagement.Services.Commands
{
    /// <summary>
    /// Interface to manage user data (Write) that interacts with the user aggregate repositories through the unit of work.
    /// </summary>
    public interface ICommandUserServicecs
    {
        /// <summary>
        /// Adds a new User entity to the data store.
        /// </summary>
        /// <param name="user">The User entity to add.</param>
        Task AddUserAsync(User user);
        /// <summary>
        /// Updates an existing User entity in the data store.
        /// </summary>
        /// <param name="user">The User entity to update.</param>
        Task UpdateUserAsync(User user);
        /// <summary>
        /// Deletes an existing User entity from the data store.
        /// </summary>
        /// <param name="user">The User entity to delete.</param>
        Task DeleteUserAsync(User user);
        /// <summary>
        /// Adds a new UserFace entity to the data store.
        /// </summary>
        /// <param name="userFace">The UserFace entity to add.</param>
        Task AddUserFaceAsync(UserFace userFace);
        /// <summary>
        /// Adds multiple new UserFace entities to the data store.
        /// </summary>
        /// <param name="userFaces">An IEnumerable of UserFace entities to add.</param>
        Task AddUserFacesAsync(IEnumerable<UserFace> userFaces);
        /// <summary>
        /// Deletes an existing UserFace entity from the data store.
        /// </summary>
        /// <param name="userFace">The UserFace entity to delete.</param>
        Task DeleteUserFaceAsync(UserFace userFace);
    }
}
