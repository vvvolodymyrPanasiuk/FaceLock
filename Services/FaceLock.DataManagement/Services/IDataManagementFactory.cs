using FaceLock.DataManagement.Services.Commands;
using FaceLock.DataManagement.Services.Queries;

namespace FaceLock.DataManagement.Services
{
    /// <summary>
    /// Factory interface for creating services related to data management.
    /// </summary>
    public interface IDataServiceFactory
    {
        /// <summary>
        /// Creates an instance of ICommandDoorLockService.
        /// </summary>
        /// <returns>The created instance of ICommandDoorLockService.</returns>
        ICommandDoorLockService CreateCommandDoorLockService();
        /// <summary>
        /// Creates an instance of ICommandPlaceService.
        /// </summary>
        /// <returns>The created instance of ICommandPlaceService.</returns>
        ICommandPlaceService CreateCommandPlaceService();
        /// <summary>
        /// Creates an instance of ICommandUserService.
        /// </summary>
        /// <returns>The created instance of ICommandUserService.</returns>
        ICommandUserService CreateCommandUserService();
        /// <summary>
        /// Creates an instance of IQueryDoorLockService.
        /// </summary>
        /// <returns>The created instance of IQueryDoorLockService.</returns>
        IQueryDoorLockService CreateQueryDoorLockService();
        /// <summary>
        /// Creates an instance of IQueryPlaceService.
        /// </summary>
        /// <returns>The created instance of IQueryPlaceService.</returns>
        IQueryPlaceService CreateQueryPlaceService();
        /// <summary>
        /// Creates an instance of IQueryUserService.
        /// </summary>
        /// <returns>The created instance of IQueryUserService.</returns>
        IQueryUserService CreateQueryUserService();
        /// <summary>
        /// Creates an instance of ITokenGeneratorService.
        /// </summary>
        /// <returns>The created instance of ITokenGeneratorService.</returns>
        ITokenGeneratorService CreateTokenGeneratorService();
    }
}
