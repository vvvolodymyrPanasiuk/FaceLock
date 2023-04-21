using FaceLock.Domain.Entities.PlaceAggregate;

namespace FaceLock.DataManagement.Services
{
    public interface IPlaceService
    {
        Task<Place> GetPlaceByIdAsync(int placeId);
        Task<IEnumerable<Place>> GetAllPlacesAsync();
        Task<IEnumerable<Visit>> GetVisitsByUserIdAsync(string userId);
        Task<IEnumerable<Visit>> GetVisitsByPlaceIdAsync(int placeId);
        Task AddPlaceAsync(Place place);
        Task UpdatePlaceAsync(Place place);
        Task DeletePlaceAsync(Place place);
        Task AddVisitAsync(Visit visit);
        Task UpdateVisitAsync(Visit visit);
        Task DeleteVisitAsync(Visit visit);
    }
}
