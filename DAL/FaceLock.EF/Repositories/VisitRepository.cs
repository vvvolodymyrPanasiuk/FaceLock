using FaceLock.Domain.Entities.PlaceAggregate;
using FaceLock.Domain.Repositories;
using Microsoft.EntityFrameworkCore;


namespace FaceLock.EF.Repositories
{
    public class VisitRepository : Repository<Visit>, IVisitRepository
    {
        public VisitRepository(FaceLockDbContext context) : base(context)
        {
        }

        public async Task<List<Visit>> GetVisitsByPlaceIdAsync(int roomId)
        {
            return await _dbSet.Where(v => v.RoomId == roomId).ToListAsync();
        }

        public async Task<List<Visit>> GetVisitsByUserIdAsync(string userId)
        {               
            return await _dbSet.Where(v => v.UserId == userId).ToListAsync();
        }
    }
}
