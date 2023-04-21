using FaceLock.Domain.Repositories.DoorLockRepository;
using FaceLock.Domain.Repositories.PlaceRepository;
using FaceLock.Domain.Repositories.UserRepository;
using FaceLock.Domain.Repositories;
using FaceLock.EF.Repositories.DoorLockRepository;
using FaceLock.EF.Repositories.PlaceRepository;
using FaceLock.EF.Repositories.UserRepository;

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
            Lazy<IUserRepository> userRepository = null,
            Lazy<IUserFaceRepository> userFaceRepository = null,
            Lazy<IDoorLockAccessRepository> doorLockAccessRepository = null,
            Lazy<IDoorLockAccessTokenRepository> doorLockAccessTokenRepository = null,
            Lazy<IDoorLockHistoryRepository> doorLockHistoryRepository = null,
            Lazy<IDoorLockRepository> doorLockRepository = null,
            Lazy<IPlaceRepository> placeRepository = null,
            Lazy<IVisitRepository> visitRepository = null)
        {
            _context = context;
            _userRepository = userRepository ?? 
                new Lazy<IUserRepository>(() => new UserRepository.UserRepository(context));
            _userFaceRepository = userFaceRepository ?? 
                new Lazy<IUserFaceRepository>(() => new UserFaceRepository(context));
            _doorLockAccessRepository = doorLockAccessRepository ?? 
                new Lazy<IDoorLockAccessRepository>(() => new DoorLockAccessRepository(context));
            _doorLockAccessTokenRepository = doorLockAccessTokenRepository ?? 
                new Lazy<IDoorLockAccessTokenRepository>(() => new DoorLockAccessTokenRepository(context));
            _doorLockHistoryRepository = doorLockHistoryRepository ?? 
                new Lazy<IDoorLockHistoryRepository>(() => new DoorLockHistoryRepository(context));
            _doorLockRepository = doorLockRepository ?? 
                new Lazy<IDoorLockRepository>(() => new DoorLockRepository.DoorLockRepository(context));
            _placeRepository = placeRepository ?? 
                new Lazy<IPlaceRepository>(() => new PlaceRepository.PlaceRepository(context));
            _visitRepository = visitRepository ?? 
                new Lazy<IVisitRepository>(() => new VisitRepository(context));
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
