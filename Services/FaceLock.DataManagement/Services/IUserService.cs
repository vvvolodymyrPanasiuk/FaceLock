using FaceLock.Domain.Entities.UserAggregate;

namespace FaceLock.DataManagement.Services
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(string userId);
        Task<User> GetUserByUsernameAsync(string userName);
        Task<IEnumerable<UserFace>> GetAllUserFacesAsync(string userId);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
        Task AddUserFaceAsync(UserFace userFace);
        Task UpdateUserFaceAsync(UserFace userFace);
        Task DeleteUserFaceAsync(UserFace userFace);
    }
}
