using FaceLock.Domain.Entities.UserAggregate;


namespace FaceLock.Domain.Repositories.UserRepository
{
    /// <summary>
    /// Interface inherited from IRepository<TEntity> for working with the Users table in the database
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// A method that returns a user from the database by the user's id
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>User</returns>
        Task<User> GetByIdAsync(string userId);
        /// <summary>
        /// A method that returns a user from the database by the user's name
        /// </summary>
        /// <param name="userName">User`s name</param>
        /// <returns>User</returns>
        Task<User> GetUserByUsernameAsync(string userName);
    }
}
