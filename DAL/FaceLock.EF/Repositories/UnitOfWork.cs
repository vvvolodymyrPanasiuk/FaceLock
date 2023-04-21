using FaceLock.Domain.Repositories.DoorLockRepository;
using FaceLock.Domain.Repositories.PlaceRepository;
using FaceLock.Domain.Repositories.UserRepository;
using FaceLock.Domain.Repositories;

namespace FaceLock.EF.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FaceLockDbContext _context;
        private readonly Lazy<IUserRepository> _userRepository;
        private readonly Lazy<IUserFaceRepository> _userFaceRepository;
        private readonly Lazy<IDoorLockAccessRepository> _doorLockAccessRepository;
        private readonly Lazy<IDoorLockAccessTokenRepository> _doorLockAccessTokenRepository;
        private readonly Lazy<IDoorLockHistoryRepository> _doorLockHistoryRepository;
        private readonly Lazy<IDoorLockRepository> _doorLockRepository;
        private readonly Lazy<IPlaceRepository> _placeRepository;
        private readonly Lazy<IVisitRepository> _visitRepository;

        public UnitOfWork(
            FaceLockDbContext context, 
            Lazy<IUserRepository> userRepository,
            Lazy<IUserFaceRepository> userFaceRepository, 
            Lazy<IDoorLockAccessRepository> doorLockAccessRepository,
            Lazy<IDoorLockAccessTokenRepository> doorLockAccessTokenRepository, 
            Lazy<IDoorLockHistoryRepository> doorLockHistoryRepository,
            Lazy<IDoorLockRepository> doorLockRepository, 
            Lazy<IPlaceRepository> placeRepository, 
            Lazy<IVisitRepository> visitRepository)
        {
            _context = context;
            _userRepository = userRepository;
            _userFaceRepository = userFaceRepository;
            _doorLockAccessRepository = doorLockAccessRepository;
            _doorLockAccessTokenRepository = doorLockAccessTokenRepository;
            _doorLockHistoryRepository = doorLockHistoryRepository;
            _doorLockRepository = doorLockRepository;
            _placeRepository = placeRepository;
            _visitRepository = visitRepository;
        }

        public IUserRepository UserRepository => _userRepository.Value;
        public IUserFaceRepository UserFaceRepository => _userFaceRepository.Value;
        public IDoorLockAccessRepository DoorLockAccessRepository => _doorLockAccessRepository.Value;
        public IDoorLockAccessTokenRepository DoorLockAccessTokenRepository => _doorLockAccessTokenRepository.Value;
        public IDoorLockHistoryRepository DoorLockHistoryRepository => _doorLockHistoryRepository.Value;
        public IDoorLockRepository DoorLockRepository => _doorLockRepository.Value;
        public IPlaceRepository PlaceRepository => _placeRepository.Value;
        public IVisitRepository VisitRepository => _visitRepository.Value;

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void BeginTransaction()
        {
            _context.Database.BeginTransaction();
        }

        public async Task CommitAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public void Rollback()
        {
            _context.Database.RollbackTransaction();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
