using FaceLock.DataManagement.Services.Commands;
using FaceLock.Domain.Entities.DoorLockAggregate;
using FaceLock.Domain.Repositories;

namespace FaceLock.DataManagement.ServicesImplementations.CommandImplementations
{
    public partial class DoorLockService : ICommandDoorLockService
    {
        private readonly IUnitOfWork _unitOfWork;
        public DoorLockService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task AddDoorLockAccessAsync(UserDoorLockAccess userDoorLockAccess)
        {
            throw new NotImplementedException();
        }

        public Task AddDoorLockAccessesAsync(IEnumerable<UserDoorLockAccess> userDoorLockAccess)
        {
            throw new NotImplementedException();
        }

        public Task AddDoorLockAsync(DoorLock doorLock)
        {
            throw new NotImplementedException();
        }

        public Task AddDoorLockHistoryAsync(DoorLockHistory doorLockHistory)
        {
            throw new NotImplementedException();
        }

        public Task DeleteDoorLockAccessAsync(UserDoorLockAccess userDoorLockAccess)
        {
            throw new NotImplementedException();
        }

        public Task DeleteDoorLockAccessesAsync(IEnumerable<UserDoorLockAccess> userDoorLockAccess)
        {
            throw new NotImplementedException();
        }

        public Task DeleteDoorLockAsync(DoorLock doorLock)
        {
            throw new NotImplementedException();
        }

        public Task DeleteDoorLockHistoryAsync(DoorLockHistory doorLockHistory)
        {
            throw new NotImplementedException();
        }

        public Task UpdateDoorLockAccessAsync(UserDoorLockAccess userDoorLockAccess)
        {
            throw new NotImplementedException();
        }

        public Task UpdateDoorLockAccessesAsync(IEnumerable<UserDoorLockAccess> userDoorLockAccess)
        {
            throw new NotImplementedException();
        }

        public Task UpdateDoorLockAsync(DoorLock doorLock)
        {
            throw new NotImplementedException();
        }

        public Task UpdateDoorLockHistoryAsync(DoorLockHistory doorLockHistory)
        {
            throw new NotImplementedException();
        }
    }
}
