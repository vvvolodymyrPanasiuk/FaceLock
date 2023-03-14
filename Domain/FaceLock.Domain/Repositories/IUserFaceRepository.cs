using FaceLock.Domain.Entities.UserAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceLock.Domain.Repositories
{
    public interface IUserFaceRepository : IRepository<UserFace>
    {
        Task<List<UserFace>> GetAllUserFacesAsync(string userId);
    }
}
