using FaceLock.DataManagement.Services.Queries;
using FaceLock.Domain.Entities.UserAggregate;
using FaceLock.Domain.Repositories;

namespace FaceLock.DataManagement.ServicesImplementations.QueryImplementations
{
    public partial class UserService : IQueryUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
    }
}
