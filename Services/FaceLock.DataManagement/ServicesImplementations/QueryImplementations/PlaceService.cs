using FaceLock.DataManagement.Services.Queries;
using FaceLock.Domain.Entities.PlaceAggregate;
using FaceLock.Domain.Repositories;
using System.ComponentModel;

namespace FaceLock.DataManagement.ServicesImplementations.QueryImplementations
{
    public partial class PlaceService : IQueryPlaceService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PlaceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region PlaceRepository
        public async Task<IEnumerable<Place>> GetAllPlacesAsync()
        {
            var places = await _unitOfWork.PlaceRepository.GetAllAsync();

            if (places == null)
            {
                throw new Exception("Place not exist");
            }

            return places;
        }

        public async Task<Place> GetPlaceByIdAsync(int placeId)
        {
            var place = await _unitOfWork.PlaceRepository.GetByIdAsync(placeId);

            if (place == null)
            {
                throw new Exception("Place not exist");
            }

            return place;
        }
        #endregion

        #region VisitRepository
        public async Task<IEnumerable<Visit>> GetVisitsByPlaceIdAsync(int placeId)
        {
            var visits = await _unitOfWork.VisitRepository.GetVisitsByPlaceIdAsync(placeId);

            if (visits == null)
            {
                throw new Exception("Visits not exist");
            }

            return visits;
        }

        public async Task<IEnumerable<Visit>> GetVisitsByUserIdAsync(string userId)
        {
            var visits = await _unitOfWork.VisitRepository.GetVisitsByUserIdAsync(userId);

            if (visits == null)
            {
                throw new Exception("Visits not exist");
            }

            return visits;
        }
        #endregion
    }
}
