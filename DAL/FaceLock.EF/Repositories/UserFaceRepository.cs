using FaceLock.Domain.Entities.UserAggregate;
using FaceLock.Domain.Repositories;
using Microsoft.EntityFrameworkCore;


namespace FaceLock.EF.Repositories
{
    public class UserFaceRepository : Repository<UserFace>, IUserFaceRepository
    {
        public UserFaceRepository(FaceLockDbContext context) : base(context)
        {
        }

        public async Task<List<UserFace>> GetAllUserFacesAsync(string userId)
        {
            return await _dbSet.Where(f => f.UserId == userId).ToListAsync();
        }
    }
}
