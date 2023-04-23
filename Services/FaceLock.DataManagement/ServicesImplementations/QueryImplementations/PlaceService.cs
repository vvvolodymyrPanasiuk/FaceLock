using FaceLock.DataManagement.Services.Queries;
using FaceLock.Domain.Entities.PlaceAggregate;
using FaceLock.Domain.Repositories;

namespace FaceLock.DataManagement.ServicesImplementations.QueryImplementations
{
    public partial class PlaceService : IQueryPlaceService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PlaceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
    }
}
