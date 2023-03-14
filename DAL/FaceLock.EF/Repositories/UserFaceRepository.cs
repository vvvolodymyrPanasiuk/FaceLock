using FaceLock.Domain.Entities.UserAggregate;
using FaceLock.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
