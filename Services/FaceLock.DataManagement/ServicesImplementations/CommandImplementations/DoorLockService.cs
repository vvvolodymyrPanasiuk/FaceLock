using FaceLock.DataManagement.Services.Commands;
using FaceLock.Domain.Entities.DoorLockAggregate;
using FaceLock.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FaceLock.DataManagement.ServicesImplementations.CommandImplementations
{
    public partial class DoorLockService : ICommandDoorLockService
    {
        private readonly IUnitOfWork _unitOfWork;
        public DoorLockService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region DoorLockAccessTokenRepository
        public async Task CreateAccessTokensAsync(int doorLockId)
        {
            List<DoorLockAccessToken> doorLockAccessTokens = new List<DoorLockAccessToken>();
            
            for(int i = 0; i <= 100; i++)
            {
                doorLockAccessTokens.Add(new DoorLockAccessToken()
                {
                    DoorLockId = doorLockId,
                    AccessToken = Guid.NewGuid().ToString(), //_ITokenService
                    Utilized = false
                });
            }

            await _unitOfWork.DoorLockAccessTokenRepository.AddRangeAsync(doorLockAccessTokens);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<DoorLockAccessToken> UseUnusedAccessTokenAsync(int doorLockId)
        {
            _unitOfWork.BeginTransaction();

            var tokens = await _unitOfWork.DoorLockAccessTokenRepository.GetAccessTokenByDoorLockIdAsync(doorLockId);
            if (tokens.Where(x => x.Utilized == false).Count() <= 1)
            {
                await UpdateAccessTokensAsync(tokens);
            }
            
            var token = await tokens.FirstOrDefaultAsync(x => x.Utilized == false);
            await UpdateAccessTokenAsync(token);
            
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();
            return token;
        }

        public async Task UpdateAccessTokenAsync(DoorLockAccessToken accessToken)
        {
            await _unitOfWork.DoorLockAccessTokenRepository.UpdateAsync(new DoorLockAccessToken()
            {
                Id = accessToken.Id,
                Utilized = true,
                DoorLockId = accessToken.DoorLockId,
                AccessToken = accessToken.AccessToken,
                DoorLock = accessToken.DoorLock
            });
            //await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAccessTokensAsync(IEnumerable<DoorLockAccessToken> accessTokens)
        {
            foreach (var doorToken in accessTokens)
            {
                doorToken.Utilized = false;
            }

            await _unitOfWork.DoorLockAccessTokenRepository.UpdateRangeAsync(accessTokens);
            //await _unitOfWork.SaveChangesAsync();
        }
        #endregion

        #region DoorLock
        public async Task AddDoorLockAsync(DoorLock doorLock)
        {
            await CreateAccessTokensAsync(doorLock.Id);
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
