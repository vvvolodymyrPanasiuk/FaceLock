using FaceLock.Domain.Entities.RoomAggregate;
using FaceLock.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceLock.EF.Repositories
{
    public class RoomRepository : Repository<Room>, IRoomRepository
    {
        public RoomRepository(FaceLockDbContext context) : base(context)
        {
        }
    }
}
