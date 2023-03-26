using FaceLock.Domain.Entities.PlaceAggregate;


namespace FaceLock.Domain.Repositories
{
    /// <summary>
    /// Interface inherited from IRepository<TEntity> for working with the Places table in the database
    /// </summary>
    public interface IPlaceRepository : IRepository<Place>
    {
    }
}
