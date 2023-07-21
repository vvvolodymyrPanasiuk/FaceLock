using FaceLock.Domain.Entities.PlaceAggregate;
using FaceLock.Domain.Repositories.PlaceRepository;

namespace FaceLock.EF.MySql.Repositories.PlaceRepository
{
    public class PlaceRepository : Repository<Place>, IPlaceRepository
    {
        public PlaceRepository(FaceLockMySqlDbContext context) : base(context)
        {
        }
    }
}
