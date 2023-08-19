using FaceLock.Domain.Repositories.DoorLockRepository;
using FaceLock.Domain.Repositories.PlaceRepository;
using FaceLock.Domain.Repositories.UserRepository;

namespace FaceLock.Domain.Repositories
{
    /// <summary>
    /// The interface for the unit of work pattern that provides access to the repositories and manages transactions.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// The repository for the User entity.
        /// </summary>
        IUserRepository UserRepository { get; }
        /// <summary>
        /// The repository for the UserFace entity.
        /// </summary>
        IUserFaceRepository UserFaceRepository { get; }
        /// <summary>
        /// The repository for the DoorLockAccess entity.
        /// </summary>
        IDoorLockAccessRepository DoorLockAccessRepository { get; }
        /// <summary>
        /// The repository for the DoorLockSecurityInfo entity.
        /// </summary>
        IDoorLockSecurityInfoRepository DoorLockSecurityInfoRepository { get; }
        /// <summary>
        /// The repository for the DoorLockHistory entity.
        /// </summary>
        IDoorLockHistoryRepository DoorLockHistoryRepository { get; }
        /// <summary>
        /// The repository for the DoorLock entity.
        /// </summary>
        IDoorLockRepository DoorLockRepository { get; }
        /// <summary>
        /// The repository for the Place entity.
        /// </summary>
        IPlaceRepository PlaceRepository { get; }
        /// <summary>
        /// The repository for the Visit entity.
        /// </summary>
        IVisitRepository VisitRepository { get; }

        /// <summary>
        /// Saves changes to the database asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation and the number of state entries written to the database.</returns>
        Task<int> SaveChangesAsync();
        /// <summary>
        /// Begins a new transaction.
        /// </summary>
        void BeginTransaction();
        /// <summary>
        /// Commits the changes made in the transaction asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CommitAsync();
        /// <summary>
        /// Rolls back the changes made in the transaction.
        /// </summary>
        void Rollback();
    }
}
