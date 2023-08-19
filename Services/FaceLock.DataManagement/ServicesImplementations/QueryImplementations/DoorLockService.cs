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
            var doorLocks = await _unitOfWork.DoorLockRepository.GetAllAsync();

            if(doorLocks == null)
            {
                throw new Exception("Door locks not exist");
            }

            return doorLocks;
        }

        public async Task<DoorLock> GetDoorLockByIdAsync(int doorLockId)
        {
            var doorLock = await _unitOfWork.DoorLockRepository.GetByIdAsync(doorLockId);

            if (doorLock == null)
            {
                throw new Exception("Door lock not exist");
            }

            return doorLock;
        }
        #endregion

        #region DoorLockAccessRepository
        public async Task<IEnumerable<UserDoorLockAccess>> GetAccessByDoorLockIdAsync(int doorLockId)
        {
            var userDoorLockAccesses = await _unitOfWork.DoorLockAccessRepository.GetAccessByDoorLockIdAsync(doorLockId);

            if(userDoorLockAccesses == null)
            {
                throw new Exception("User accesses not exist");
            }

            return userDoorLockAccesses;
        }

        public async Task<IEnumerable<UserDoorLockAccess>> GetAccessByUserIdAsync(string userId)
        {
            var userDoorLockAccesses = await _unitOfWork.DoorLockAccessRepository.GetAccessByUserIdAsync(userId);

            if (userDoorLockAccesses == null)
            {
                throw new Exception("User accesses not exist");
            }

            return userDoorLockAccesses;
        }

        public async Task<UserDoorLockAccess> GetUserDoorLockAccessByIdAsync(int userDoorLockAccessId)
        {
            var userDoorLockAccess = await _unitOfWork.DoorLockAccessRepository.GetByIdAsync(userDoorLockAccessId);

            if (userDoorLockAccess == null)
            {
                throw new Exception("User access not exist");
            }

            return userDoorLockAccess;
        }

        public async Task<UserDoorLockAccess> GetUserDoorLockAccessByIdAsync(string userId, int doorLockId)
        {
            var accessesDoorLock = await _unitOfWork.DoorLockAccessRepository.GetAccessByDoorLockIdAsync(doorLockId);
            var accessDoorLock = accessesDoorLock.FirstOrDefault(a => a.UserId == userId);

            if (accessDoorLock == null)
            {
                throw new Exception("User access not exist");
            }

            return accessDoorLock;
        }

        #endregion

        #region DoorLockHistoryRepository
        public async Task<IEnumerable<DoorLockHistory>> GetDoorLockHistoryByDoorLockIdAsync(int doorLockId)
        {
            var doorLockHistories = await _unitOfWork.DoorLockHistoryRepository.GetDoorLockHistoryByDoorLockIdAsync(doorLockId);

            if (doorLockHistories == null)
            {
                throw new Exception("Door lock histories not exist");
            }

            return doorLockHistories;
        }

        public async Task<DoorLockHistory> GetDoorLockHistoryByIdAsync(int doorLockHistoryId)
        {
            var doorLockHistory = await _unitOfWork.DoorLockHistoryRepository.GetByIdAsync(doorLockHistoryId);

            if (doorLockHistory == null)
            {
                throw new Exception("Door lock history not exist");
            }

            return doorLockHistory;
        }

        public async Task<IEnumerable<DoorLockHistory>> GetDoorLockHistoryByUserIdAsync(string userId)
        {
            var doorLockHistories = await _unitOfWork.DoorLockHistoryRepository.GetDoorLockHistoryByUserIdAsync(userId);

            if (doorLockHistories == null)
            {
                throw new Exception("Door lock histories not exist");
            }

            return doorLockHistories;
        }
        #endregion

        #region DoorLockSecurityInfoRepository
        public async Task<DoorLockSecurityInfo> GetSecurityInfoByDoorLockIdAsync(int doorLockId)
        {
            var doorLockSecurityInfo = await _unitOfWork.DoorLockSecurityInfoRepository.GetByIdAsync(doorLockId);

            if (doorLockSecurityInfo == null)
            {
                throw new Exception("Door lock security information not exist");
            }

            return doorLockSecurityInfo;
        }
        #endregion
    }
}
