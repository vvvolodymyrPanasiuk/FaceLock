using FaceLock.Domain.Entities.UserAggregate;


namespace FaceLock.Domain.Repositories.UserRepository
{
    /// <summary>
    /// Interface inherited from IRepository for working with the UserFaces table in the database
    /// </summary>
    public interface IUserFaceRepository : IRepository<UserFace>
    {
        /// <summary>
        /// A method that returns a a list of user`s faces from the database by the user's id
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>List of UserFace entities from the database</returns>
        Task<IQueryable<UserFace>> GetAllUserFacesAsync(string userId);
    }
}
