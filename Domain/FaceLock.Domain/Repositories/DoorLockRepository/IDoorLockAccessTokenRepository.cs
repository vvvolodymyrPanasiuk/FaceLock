using FaceLock.Domain.Entities.DoorLockAggregate;

namespace FaceLock.Domain.Repositories.DoorLockRepository
{
    /// <summary>
    /// Interface inherited from IRepository for working with the DoorLockAccessToken table in the database
    /// </summary>
    public interface IDoorLockAccessTokenRepository : IRepository<DoorLockAccessToken>
    {
        ///  <summary> 
        /// A method that retrieves a list of access tokens based on a given door lock ID.
        ///  </summary> 
        ///  <param name="doorLockId">Door lock id</param> 
        ///  <returns>List of DoorLockAccessToken entities from the database</returns> 
        Task<IQueryable<DoorLockAccessToken>> GetAccessTokenByDoorLockIdAsync(int doorLockId);
    }
}
