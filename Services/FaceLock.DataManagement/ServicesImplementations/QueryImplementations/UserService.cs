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

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _unitOfWork.UserRepository.GetAllAsync();
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _unitOfWork.UserRepository.GetByIdAsync(userId);
        }

        public async Task<User> GetUserByUsernameAsync(string userName)
        {
            return await _unitOfWork.UserRepository.GetUserByUsernameAsync(userName);
        }

        public async Task<UserFace> GetUserFaceByIdAsync(string userId, int faceId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if(user == null)
            {
                throw new Exception("User not exists");
            }

            var userFace = await _unitOfWork.UserFaceRepository.GetByIdAsync(faceId);
            if (userFace == null)
            {
                throw new Exception("UserFace not exists");
            }
            return userFace;
        }

        public async Task<IEnumerable<UserFace>> GetUserFacesByIdAsync(string userId, IEnumerable<int> facesId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not exists");
            }
            var userFaces = new List<UserFace>();
            foreach (var faceId in facesId)
            {
                userFaces.Add(await _unitOfWork.UserFaceRepository.GetByIdAsync(faceId));
            }
            if (userFaces == null)
            {
                throw new Exception("UserFaces not exists");
            }
            return userFaces;
        }

        public async Task<IEnumerable<User>> GetUsersByIdAsync(IEnumerable<string> usersId)
        {
            var usersResult = new List<User>();
            foreach(var userId in usersId)
            {
                usersResult.Add(await _unitOfWork.UserRepository.GetByIdAsync(userId));
            }

            return usersResult;
        }
    }
}
