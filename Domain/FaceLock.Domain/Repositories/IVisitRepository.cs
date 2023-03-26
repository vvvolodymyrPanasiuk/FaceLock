﻿using FaceLock.Domain.Entities.PlaceAggregate;


namespace FaceLock.Domain.Repositories
{
    /// <summary>
    /// Interface inherited from IRepository<TEntity> for working with the Visits table in the database
    /// </summary>
    public interface IVisitRepository : IRepository<Visit>
    {
        /// <summary>
        /// A method that returns a a list of visits from the database by the user's id
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>List<Visit></returns>
        Task<List<Visit>> GetVisitsByUserIdAsync(string userId);
        /// <summary>
        /// A method that returns a a list of visits from the database by the place's id
        /// </summary>
        /// <param name="placeId">Place id</param>
        /// <returns>List<Visit></returns>
        Task<List<Visit>> GetVisitsByPlaceIdAsync(int placeId);
    }
}
