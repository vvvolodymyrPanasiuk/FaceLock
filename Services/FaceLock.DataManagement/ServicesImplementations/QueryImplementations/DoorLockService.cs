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

        public Task<IEnumerable<UserDoorLockAccess>> GetAccessByDoorLockIdAsync(int doorLockId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserDoorLockAccess>> GetAccessByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DoorLockAccessToken>> GetAccessTokenByDoorLockIdAsync(int doorLockId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DoorLock>> GetAllDoorLocksAsync()
        {
            throw new NotImplementedException();
        }

        public Task<DoorLock> GetDoorLockByIdAsync(int doorLockId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DoorLockHistory>> GetDoorLockHistoryByDoorLockIdAsync(int doorLockId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DoorLockHistory>> GetDoorLockHistoryByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
