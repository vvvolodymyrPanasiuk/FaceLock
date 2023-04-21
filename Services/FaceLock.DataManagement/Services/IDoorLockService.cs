using FaceLock.Domain.Entities.DoorLockAggregate;

namespace FaceLock.DataManagement.Services
{
    public interface IDoorLockService
    {
        Task<IEnumerable<UserDoorLockAccess>> GetAccessByUserIdAsync(string userId);
        Task<IEnumerable<UserDoorLockAccess>> GetAccessByDoorLockIdAsync(int doorLockId);
        Task<IEnumerable<DoorLockAccessToken>> GetAccessTokenByDoorLockIdAsync(int doorLockId);
        Task<IEnumerable<DoorLockHistory>> GetDoorLockHistoryByUserIdAsync(string userId);
        Task<IEnumerable<DoorLockHistory>> GetDoorLockHistoryByDoorLockIdAsync(int doorLockId);
        Task<DoorLock> GetDoorLockByIdAsync(int doorLockId);
        Task<IEnumerable<DoorLock>> GetAllDoorLocksAsync();
        Task AddDoorLockAsync(DoorLock doorLock);
        Task UpdateDoorLockAsync(DoorLock doorLock);
        Task DeleteDoorLockAsync(DoorLock doorLock);
        Task AddDoorLockAccessAsync(UserDoorLockAccess userDoorLockAccess);
        Task UpdateDoorLockAccessAsync(UserDoorLockAccess userDoorLockAccess);
        Task DeleteDoorLockAccessAsync(UserDoorLockAccess userDoorLockAccess);
        Task AddDoorLockAccessTokenAsync(DoorLockAccessToken doorLockAccessToken);
        Task UpdateDoorLockAccessTokenAsync(DoorLockAccessToken doorLockAccessToken);
        Task DeleteDoorLockAccessTokenAsync(DoorLockAccessToken doorLockAccessToken);
        Task AddDoorLockHistoryAsync(DoorLockHistory doorLockHistory);
        Task UpdateDoorLockHistoryAsync(DoorLockHistory doorLockHistory);
        Task DeleteDoorLockHistoryAsync(DoorLockHistory doorLockHistory);
    }
}
