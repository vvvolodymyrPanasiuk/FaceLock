using FaceLock.DataManagement.Services;
using FaceLock.Domain.Entities.PlaceAggregate;
using FaceLock.Domain.Repositories;

namespace FaceLock.DataManagement.ServicesImplementations
{
    public class PlaceService : IPlaceService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PlaceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddPlaceAsync(Place place)
        {
            await _unitOfWork.PlaceRepository.AddAsync(place);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddVisitAsync(Visit visit)
        {
            await _unitOfWork.VisitRepository.AddAsync(visit);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeletePlaceAsync(Place place)
        {
            await _unitOfWork.PlaceRepository.DeleteAsync(place);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteVisitAsync(Visit visit)
        {
            await _unitOfWork.VisitRepository.DeleteAsync(visit);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<Place>> GetAllPlacesAsync()
        {
            return await _unitOfWork.PlaceRepository.GetAllAsync();
        }

        public async Task<Place> GetPlaceByIdAsync(int placeId)
        {
            return await _unitOfWork.PlaceRepository.GetByIdAsync(placeId);
        }

        public async Task<IEnumerable<Visit>> GetVisitsByPlaceIdAsync(int placeId)
        {
            return await _unitOfWork.VisitRepository.GetVisitsByPlaceIdAsync(placeId);
        }

        public async Task<IEnumerable<Visit>> GetVisitsByUserIdAsync(string userId)
        {
            return await _unitOfWork.VisitRepository.GetVisitsByUserIdAsync(userId);
        }

        public async Task UpdatePlaceAsync(Place place)
        {
            await _unitOfWork.PlaceRepository.UpdateAsync(place);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateVisitAsync(Visit visit)
        {
            await _unitOfWork.VisitRepository.UpdateAsync(visit);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
