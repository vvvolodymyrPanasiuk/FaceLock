using FaceLock.Domain.Entities.PlaceAggregate;
using FaceLock.Domain.Repositories.PlaceRepository;


namespace FaceLock.EF.MySql.Repositories.PlaceRepository
{
    public class VisitRepository : Repository<Visit>, IVisitRepository
    {
        public VisitRepository(FaceLockMySqlDbContext context) : base(context)
        {
        }

        public async Task<IQueryable<Visit>> GetVisitsByPlaceIdAsync(int placeId)
        {
            return _dbSet.Where(x => x.PlaceId == placeId);
        }

        public async Task<IQueryable<Visit>> GetVisitsByUserIdAsync(string userId)
        {
            return _dbSet.Where(v => v.UserId == userId);
        }
    }
}
