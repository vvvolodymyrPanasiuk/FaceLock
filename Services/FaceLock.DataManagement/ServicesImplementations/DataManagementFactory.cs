using FaceLock.DataManagement.Services.Commands;
using FaceLock.DataManagement.Services.Queries;
using FaceLock.DataManagement.Services;
using FaceLock.DataManagement.ServicesImplementations.TokenGeneratorImplementation;
using FaceLock.Domain.Repositories;

namespace FaceLock.DataManagement.ServicesImplementations
{
    public class DataServiceFactory : IDataServiceFactory
    {
        private readonly IUnitOfWork _context;
        private readonly Lazy<ICommandDoorLockService> _commandDoorLockService;
        private readonly Lazy<ICommandPlaceService> _commandPlaceService;
        private readonly Lazy<ICommandUserService> _commandUserService;
        private readonly Lazy<IQueryDoorLockService> _queryDoorLockService;
        private readonly Lazy<IQueryPlaceService> _queryPlaceService;
        private readonly Lazy<IQueryUserService> _queryUserService;
        private readonly Lazy<ISecretKeyGeneratorService> _secretKeyGeneratorService;
        private readonly Lazy<ITokenGeneratorService> _tokenGeneratorService;

        public DataServiceFactory(
            IUnitOfWork contextFactory,
            Lazy<ISecretKeyGeneratorService> secretKeyGeneratorService = null,
            Lazy<IQueryUserService> queryUserService = null,
            Lazy<IQueryPlaceService> queryPlaceService = null,
            Lazy<IQueryDoorLockService> queryDoorLockService = null, 
            Lazy<ICommandUserService> commandUserService = null,
            Lazy<ICommandPlaceService> commandPlaceService = null,
            Lazy<ICommandDoorLockService> commandDoorLockService = null,
            Lazy<ITokenGeneratorService> tokenGeneratorService = null)
        {
            _context = contextFactory;
            _secretKeyGeneratorService = secretKeyGeneratorService ?? 
                new Lazy<ISecretKeyGeneratorService>(() => new SecureRandomSecretKeyGeneratorStrategy());
            _tokenGeneratorService = tokenGeneratorService ?? 
                new Lazy<ITokenGeneratorService>(() => new JwtTokenGeneratorService());
            _queryUserService = queryUserService ??
                new Lazy<IQueryUserService>(() => new QueryImplementations.UserService(_context));
            _queryPlaceService = queryPlaceService ??
                new Lazy<IQueryPlaceService>(() => new QueryImplementations.PlaceService(_context));
            _queryDoorLockService = queryDoorLockService ??
                new Lazy<IQueryDoorLockService>(() => new QueryImplementations.DoorLockService(_context, _tokenGeneratorService.Value));
            _commandUserService = commandUserService ??
                new Lazy<ICommandUserService>(() => new CommandImplementations.UserService(_context));
            _commandPlaceService = commandPlaceService ?? 
                new Lazy<ICommandPlaceService>(() => new CommandImplementations.PlaceService(_context));
            _commandDoorLockService = commandDoorLockService ?? 
                new Lazy<ICommandDoorLockService>(() => 
                new CommandImplementations.DoorLockService(_context, _secretKeyGeneratorService.Value));                     
        }

        public ICommandDoorLockService CreateCommandDoorLockService()
        {
            return _commandDoorLockService.Value;
        }

        public ICommandPlaceService CreateCommandPlaceService()
        {
            return _commandPlaceService.Value;
        }

        public ICommandUserService CreateCommandUserService()
        {
            return _commandUserService.Value;
        }

        public IQueryDoorLockService CreateQueryDoorLockService()
        {
            return _queryDoorLockService.Value;
        }

        public IQueryPlaceService CreateQueryPlaceService()
        {
            return _queryPlaceService.Value;
        }

        public IQueryUserService CreateQueryUserService()
        {
            return _queryUserService.Value;
        }

        public ISecretKeyGeneratorService CreateSecretKeyGeneratorService()
        {
            return _secretKeyGeneratorService.Value;
        }
    }
}
