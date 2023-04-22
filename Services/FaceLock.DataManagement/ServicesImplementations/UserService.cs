using FaceLock.DataManagement.Services;
using FaceLock.Domain.Entities.UserAggregate;
using FaceLock.Domain.Repositories;

namespace FaceLock.DataManagement.ServicesImplementations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddUserAsync(User user)
        {
            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddUserFaceAsync(UserFace userFace)
        {
            await _unitOfWork.UserFaceRepository.AddAsync(userFace);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddUserFacesAsync(IEnumerable<UserFace> userFaces)
        {
            foreach(var userFace in  userFaces)
            {
                await _unitOfWork.UserFaceRepository.AddAsync(userFace);
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(User user)
        {
            await _unitOfWork.UserRepository.DeleteAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteUserFaceAsync(UserFace userFace)
        {
            await _unitOfWork.UserFaceRepository.DeleteAsync(userFace);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserFace>> GetAllUserFacesAsync(string userId)
        {
            return await _unitOfWork.UserFaceRepository.GetAllUserFacesAsync(userId);
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _unitOfWork.UserRepository.GetByIdAsync(userId);
        }

        public async Task<User> GetUserByUsernameAsync(string userName)
        {
            return await _unitOfWork.UserRepository.GetUserByUsernameAsync(userName);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
