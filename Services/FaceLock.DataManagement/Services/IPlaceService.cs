using FaceLock.Domain.Entities.PlaceAggregate;

namespace FaceLock.DataManagement.Services
{
    /// <summary>
    /// Interface to manage place data that interacts with the place aggregate repositories through the unit of work.
    /// </summary>
    public interface IPlaceService
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
        /// <summary>
        /// Add a new Place entity.
        /// </summary>
        /// <param name="place">The Place entity to add.</param>
        Task AddPlaceAsync(Place place);
        /// <summary>
        /// Update an existing Place entity.
        /// </summary>
        /// <param name="place">The Place entity to update.</param>
        Task UpdatePlaceAsync(Place place);
        /// <summary>
        /// Delete a Place entity.
        /// </summary>
        /// <param name="place">The Place entity to delete.</param>
        Task DeletePlaceAsync(Place place);
        /// <summary>
        /// Add a new Visit entity.
        /// </summary>
        /// <param name="visit">The Visit entity to add.</param>
        Task AddVisitAsync(Visit visit);
        /// <summary>
        /// Update an existing Visit entity.
        /// </summary>
        /// <param name="visit">The Visit entity to update.</param>
        Task UpdateVisitAsync(Visit visit);
        /// <summary>
        /// Delete a Visit entity.
        /// </summary>
        /// <param name="visit">The Visit entity to delete.</param>
        Task DeleteVisitAsync(Visit visit);
    }
}
