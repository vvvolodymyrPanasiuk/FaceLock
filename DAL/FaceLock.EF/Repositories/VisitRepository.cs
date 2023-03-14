using FaceLock.Domain.Entities.RoomAggregate;
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
    public class VisitRepository : Repository<Visit>, IVisitRepository
    {
        public VisitRepository(FaceLockDbContext context) : base(context)
        {
        }

        public async Task<List<Visit>> GetVisitsByRoomIdAsync(int roomId)
        {
            return await _dbSet.Where(v => v.RoomId == roomId).ToListAsync();
        }

        public async Task<List<Visit>> GetVisitsByUserIdAsync(string userId)
        {               
            return await _dbSet.Where(v => v.UserId == userId).ToListAsync();
        }
    }
}
