using FaceLock.Domain.Entities.UserAggregate;


namespace FaceLock.Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByIdAsync(string id);
        Task<User> GetUserByUsernameAsync(string username);
    }
}
