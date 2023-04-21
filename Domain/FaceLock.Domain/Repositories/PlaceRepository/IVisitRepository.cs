using FaceLock.Domain.Entities.PlaceAggregate;


namespace FaceLock.Domain.Repositories.PlaceRepository
{
    /// <summary>
    /// Interface inherited from IRepository for working with the Visits table in the database
    /// </summary>
    public interface IVisitRepository : IRepository<Visit>
    {
        /// <summary>
        /// A method that returns a a list of visits from the database by the user's id
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>List of Visit entities from the database</returns>
        Task<IQueryable<Visit>> GetVisitsByUserIdAsync(string userId);
        /// <summary>
        /// A method that returns a a list of visits from the database by the place's id
        /// </summary>
        /// <param name="placeId">Place id</param>
        /// <returns>List of Visit entities from the database</returns>
        Task<IQueryable<Visit>> GetVisitsByPlaceIdAsync(int placeId);
    }
}