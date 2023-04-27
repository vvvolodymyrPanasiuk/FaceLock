using FaceLock.DataManagement.Services.Commands;
using FaceLock.Domain.Entities.PlaceAggregate;
using FaceLock.Domain.Repositories;

namespace FaceLock.DataManagement.ServicesImplementations.CommandImplementations
{
    public partial class PlaceService : ICommandPlaceService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PlaceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region PlaceRepository
        public async Task AddPlaceAsync(Place place)
        {
            await _unitOfWork.PlaceRepository.AddAsync(place);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeletePlaceAsync(Place place)
        {
            await _unitOfWork.PlaceRepository.DeleteAsync(place);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdatePlaceAsync(Place place)
        {
            await _unitOfWork.PlaceRepository.UpdateAsync(place);
            await _unitOfWork.SaveChangesAsync();
        }
        #endregion

        #region VisitRepository
        public async Task AddVisitAsync(Visit visit)
        {
            await _unitOfWork.VisitRepository.AddAsync(visit);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteVisitAsync(Visit visit)
        {
            await _unitOfWork.VisitRepository.DeleteAsync(visit);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateVisitAsync(Visit visit)
        {
            await _unitOfWork.VisitRepository.UpdateAsync(visit);
            await _unitOfWork.SaveChangesAsync();
        }
        #endregion
    }
}
