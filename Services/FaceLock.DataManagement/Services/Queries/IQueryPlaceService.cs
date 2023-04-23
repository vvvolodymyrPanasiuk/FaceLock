using FaceLock.Domain.Entities.PlaceAggregate;

namespace FaceLock.DataManagement.Services.Queries
{
    /// <summary>
    /// Interface to manage place data (Read) that interacts with the place aggregate repositories through the unit of work.
    /// </summary>
    public interface IQueryPlaceService
    {
        /// <summary>
        /// Get a Place entity by its ID.
        /// </summary>
        /// <param name="placeId">The ID of the Place entity.</param>
        /// <returns>The Place entity.</returns>
        Task<Place> GetPlaceByIdAsync(int placeId);
        /// <summary>
        /// Get all Place entities.
        /// </summary>
        /// <returns>A collection of Place entities.</returns>
        Task<IEnumerable<Place>> GetAllPlacesAsync();
        /// <summary>
        /// Get all Visit entities associated with a given User entity.
        /// </summary>
        /// <param name="userId">The ID of the User entity.</param>
        /// <returns>A collection of Visit entities.</returns>
        Task<IEnumerable<Visit>> GetVisitsByUserIdAsync(string userId);
        /// <summary>
        /// Get all Visit entities associated with a given Place entity.
        /// </summary>
        /// <param name="placeId">The ID of the Place entity.</param>
        /// <returns>A collection of Visit entities.</returns>
        Task<IEnumerable<Visit>> GetVisitsByPlaceIdAsync(int placeId);
    }
}
