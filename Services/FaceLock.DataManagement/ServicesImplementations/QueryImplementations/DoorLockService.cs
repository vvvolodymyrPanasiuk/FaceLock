using FaceLock.DataManagement.Services.Queries;
using FaceLock.Domain.Entities.DoorLockAggregate;
using FaceLock.Domain.Repositories;

namespace FaceLock.DataManagement.ServicesImplementations.QueryImplementations
{
    public partial class DoorLockService : IQueryDoorLockService
    {
        private readonly IUnitOfWork _unitOfWork;
        public DoorLockService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region DoorLockRepository
        public async Task<IEnumerable<DoorLock>> GetAllDoorLocksAsync()
        {
            return await _unitOfWork.DoorLockRepository.GetAllAsync();
        }

        public async Task<DoorLock> GetDoorLockByIdAsync(int doorLockId)
        {
            return await _unitOfWork.DoorLockRepository.GetByIdAsync(doorLockId);
        }
        #endregion

        #region DoorLockAccessRepository
        public async Task<IEnumerable<UserDoorLockAccess>> GetAccessByDoorLockIdAsync(int doorLockId)
        {
            return await _unitOfWork.DoorLockAccessRepository.GetAccessByDoorLockIdAsync(doorLockId);
        }

        public async Task<IEnumerable<UserDoorLockAccess>> GetAccessByUserIdAsync(string userId)
        {
            return await _unitOfWork.DoorLockAccessRepository.GetAccessByUserIdAsync(userId);
        }

        public async Task<UserDoorLockAccess> GetUserDoorLockAccessByIdAsync(int userDoorLockAccessId)
        {
            return await _unitOfWork.DoorLockAccessRepository.GetByIdAsync(userDoorLockAccessId);
        }
        #endregion

        #region DoorLockHistoryRepository
        public async Task<IEnumerable<DoorLockHistory>> GetDoorLockHistoryByDoorLockIdAsync(int doorLockId)
        {
            return await _unitOfWork.DoorLockHistoryRepository.GetDoorLockHistoryByDoorLockIdAsync(doorLockId);
        }

        public async Task<DoorLockHistory> GetDoorLockHistoryByIdAsync(int doorLockHistoryId)
        {
            return await _unitOfWork.DoorLockHistoryRepository.GetByIdAsync(doorLockHistoryId);
        }

        public async Task<IEnumerable<DoorLockHistory>> GetDoorLockHistoryByUserIdAsync(string userId)
        {
            return await _unitOfWork.DoorLockHistoryRepository.GetDoorLockHistoryByUserIdAsync(userId);
        }
        #endregion

        #region DoorLockAccessTokenRepository
        public async Task<IEnumerable<DoorLockAccessToken>> GetAccessTokensByDoorLockIdAsync(int doorLockId)
        {
            return await _unitOfWork.DoorLockAccessTokenRepository.GetAccessTokenByDoorLockIdAsync(doorLockId);
        }
        #endregion
    }
}
