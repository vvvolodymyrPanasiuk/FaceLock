using FaceLock.Domain.Entities.PlaceAggregate;
using FaceLock.Domain.Repositories.PlaceRepository;
using Microsoft.EntityFrameworkCore;


namespace FaceLock.EF.Repositories.PlaceRepository
{
    public class VisitRepository : Repository<Visit>, IVisitRepository
    {
        public VisitRepository(FaceLockDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Visit>> GetVisitsByPlaceIdAsync(int placeId)
        {
            return await _dbSet.Where(v => v.PlaceId == placeId).ToListAsync();
        }

        public async Task<IEnumerable<Visit>> GetVisitsByUserIdAsync(string userId)
        {
            return await _dbSet.Where(v => v.UserId == userId).ToListAsync();
        }
    }
}
