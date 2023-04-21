using FaceLock.Domain.Entities.UserAggregate;
using FaceLock.Domain.Repositories.UserRepository;
using Microsoft.EntityFrameworkCore;


namespace FaceLock.EF.Repositories.UserRepository
{
    public class UserFaceRepository : Repository<UserFace>, IUserFaceRepository
    {
        public UserFaceRepository(FaceLockDbContext context) : base(context)
        {
        }

        public async Task<IQueryable<UserFace>> GetAllUserFacesAsync(string userId)
        {
            return _dbSet.Where(uf => uf.UserId == userId);
        }
    }
}
