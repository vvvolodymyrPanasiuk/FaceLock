using FaceLock.Domain.Entities.UserAggregate;
using FaceLock.Domain.Repositories.UserRepository;
using Microsoft.EntityFrameworkCore;


namespace FaceLock.EF.MySql.Repositories.UserRepository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(FaceLockMySqlDbContext context) : base(context)
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
