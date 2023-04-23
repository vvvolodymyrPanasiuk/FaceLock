using FaceLock.Domain.Entities.PlaceAggregate;

namespace FaceLock.DataManagement.Services.Commands
{
    /// <summary>
    /// Interface to manage place data (Write) that interacts with the place aggregate repositories through the unit of work.
    /// </summary>
    public interface ICommandPlaceService
    {
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
