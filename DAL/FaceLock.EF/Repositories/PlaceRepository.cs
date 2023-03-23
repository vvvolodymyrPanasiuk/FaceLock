using FaceLock.Domain.Entities.PlaceAggregate;
using FaceLock.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceLock.EF.Repositories
{
    public class PlaceRepository : Repository<Place>, IPlaceRepository
    {
        public PlaceRepository(FaceLockDbContext context) : base(context)
        {
        }
    }
}
