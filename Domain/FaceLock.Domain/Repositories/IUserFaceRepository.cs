using FaceLock.Domain.Entities.UserAggregate;


namespace FaceLock.Domain.Repositories
{
    public interface IUserFaceRepository : IRepository<UserFace>
    {
        Task<List<UserFace>> GetAllUserFacesAsync(string userId);
    }
}
