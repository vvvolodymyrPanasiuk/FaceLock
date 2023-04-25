using FaceLock.Domain.Entities.UserAggregate;
using Microsoft.AspNetCore.Http;

namespace FaceLock.DataManagement.Services.Commands
{
    /// <summary>
    /// Interface to manage user data (Write) that interacts with the user aggregate repositories through the unit of work.
    /// </summary>
    public interface ICommandUserService
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
        /// Deletes an existing User entities from the data store.
        /// </summary>
        /// <param name="users">The User entities to delete.</param>
        Task DeleteUsersAsync(IEnumerable<User> users);
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
        /// <summary>
        /// Deletes an existing UserFace entities from the data store.
        /// </summary>
        /// <param name="userFaces">The list of UserFace entities to delete.</param>
        Task DeleteUserFacesAsync(IEnumerable<UserFace> userFaces);
        /// <summary>
        /// Adds a new UserFace entity to the data store.
        /// </summary>
        /// <param name="userId">The user ID entity to add face.</param>
        /// <param name="userFace">The UserFace file to add.</param>
        Task AddUserFaceAsync(string userId, IFormFile userFace);
        /// <summary>
        /// Adds a new UserFace entities to the data store.
        /// </summary>
        /// <param name="userId">The user ID entity to add face.</param>
        /// <param name="userFaces">The UserFace files to add.</param>
        Task AddUserFacesAsync(string userId, IFormFileCollection userFaces);
    }
}
