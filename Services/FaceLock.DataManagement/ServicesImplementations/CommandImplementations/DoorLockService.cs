using FaceLock.DataManagement.Services;
using FaceLock.DataManagement.Services.Commands;
using FaceLock.Domain.Entities.DoorLockAggregate;
using FaceLock.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FaceLock.DataManagement.ServicesImplementations.CommandImplementations
{
    public partial class DoorLockService : ICommandDoorLockService
    {
        private readonly ISecretKeyGeneratorService _secretKeyGeneratorService;
        private readonly IUnitOfWork _unitOfWork;
        public DoorLockService(IUnitOfWork unitOfWork, ISecretKeyGeneratorService secretKeyGeneratorService)
        {
            _secretKeyGeneratorService = secretKeyGeneratorService;
            _unitOfWork = unitOfWork;
        }

        #region DoorLockSecurityInfoRepository
        public async Task CreateSecurityInfoAsync(int doorLockId, string urlConnection)
        {      
            await _unitOfWork.DoorLockSecurityInfoRepository.AddAsync(
                new DoorLockSecurityInfo()
                    {
                        DoorLockId = doorLockId,
                        UrlConnection = urlConnection,
                        SecretKey = _secretKeyGeneratorService.GenerateSecretKey()
                    });
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateSecurityInfoAsync(DoorLockSecurityInfo securityInfo)
        {
            await _unitOfWork.DoorLockSecurityInfoRepository.UpdateAsync(new DoorLockSecurityInfo()
            {
                Id = securityInfo.Id,
                DoorLockId = securityInfo.DoorLockId,
                SecretKey = securityInfo.SecretKey,
                UrlConnection = securityInfo.UrlConnection
            });
            await _unitOfWork.SaveChangesAsync();
        }
        #endregion

        #region DoorLock
        public async Task AddDoorLockAsync(DoorLock doorLock)
        {
            await _unitOfWork.DoorLockRepository.AddAsync(doorLock);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteDoorLockAsync(DoorLock doorLock)
        {
            await _unitOfWork.DoorLockRepository.DeleteAsync(doorLock);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateDoorLockAsync(DoorLock doorLock)
        {
            await _unitOfWork.DoorLockRepository.UpdateAsync(doorLock);
            await _unitOfWork.SaveChangesAsync();
        }  
        #endregion

        #region DoorLockHistoryRepository
        public async Task AddDoorLockHistoryAsync(DoorLockHistory doorLockHistory)
        {
            await _unitOfWork.DoorLockHistoryRepository.AddAsync(doorLockHistory);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteDoorLockHistoryAsync(DoorLockHistory doorLockHistory)
        {
            await _unitOfWork.DoorLockHistoryRepository.DeleteAsync(doorLockHistory);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateDoorLockHistoryAsync(DoorLockHistory doorLockHistory)
        {
            await _unitOfWork.DoorLockHistoryRepository.UpdateAsync(doorLockHistory);
            await _unitOfWork.SaveChangesAsync();
        }
        #endregion

        #region DoorLockAccessRepository
        public async Task AddDoorLockAccessAsync(UserDoorLockAccess userDoorLockAccess)
        {
            await _unitOfWork.DoorLockAccessRepository.AddAsync(userDoorLockAccess);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddDoorLockAccessesAsync(IEnumerable<UserDoorLockAccess> userDoorLockAccess)
        {
            await _unitOfWork.DoorLockAccessRepository.AddRangeAsync(userDoorLockAccess);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteDoorLockAccessAsync(UserDoorLockAccess userDoorLockAccess)
        {
            await _unitOfWork.DoorLockAccessRepository.DeleteAsync(userDoorLockAccess);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteDoorLockAccessesAsync(IEnumerable<UserDoorLockAccess> userDoorLockAccess)
        {
            await _unitOfWork.DoorLockAccessRepository.DeleteRangeAsync(userDoorLockAccess);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateDoorLockAccessAsync(UserDoorLockAccess userDoorLockAccess)
        {
            await _unitOfWork.DoorLockAccessRepository.UpdateAsync(userDoorLockAccess);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateDoorLockAccessesAsync(IEnumerable<UserDoorLockAccess> userDoorLockAccess)
        {
            await _unitOfWork.DoorLockAccessRepository.UpdateRangeAsync(userDoorLockAccess);
            await _unitOfWork.SaveChangesAsync();
        } 
        #endregion
    }
}
