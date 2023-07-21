using FaceLock.Domain.Entities.UserAggregate;
using FaceLock.Domain.Repositories.UserRepository;


namespace FaceLock.EF.MySql.Repositories.UserRepository
{
    public class UserFaceRepository : Repository<UserFace>, IUserFaceRepository
    {
        public UserFaceRepository(FaceLockMySqlDbContext context) : base(context)
        {
        }

        public async Task<IQueryable<UserFace>> GetAllUserFacesAsync(string userId)
        {
            return _dbSet.Where(uf => uf.UserId == userId);
        }
    }
}
