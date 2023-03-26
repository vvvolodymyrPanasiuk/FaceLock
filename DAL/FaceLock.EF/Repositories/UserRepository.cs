using FaceLock.Domain.Entities.UserAggregate;
using FaceLock.Domain.Repositories;
using Microsoft.EntityFrameworkCore;


namespace FaceLock.EF.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(FaceLockDbContext context) : base(context)
        {
        }

        public async Task<User> GetByIdAsync(string userId)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Id == userId);
        }
        public async Task<User> GetUserByUsernameAsync(string userName)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.UserName == userName);
        }
    }
}
