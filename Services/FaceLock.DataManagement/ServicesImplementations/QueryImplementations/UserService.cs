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


        #region UserRepository
        public async Task<User> GetUserByIdAsync(string userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            
            if (user == null)
            {
                throw new Exception("User not exists");
            }

            return user;
        }
        
        public async Task<User> GetUserByEmailAsync(string userEmail)
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();
            var user = users.FirstOrDefault(e => e.Email == userEmail);
            
            if (user == null)
            {
                throw new Exception("User not exists");
            }

            return user;
        }

        public async Task<User> GetUserByUsernameAsync(string userName)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(userName);

            if (user == null)
            {
                throw new Exception("User not exists");
            }

            return user;
        }

        public async Task<bool> IsExistUserByEmailAsync(string userEmail)
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();
            var user = users.FirstOrDefault(e => e.Email == userEmail);

            if (user == null)
            {
                return false;
            }

            return true;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();

            if (users == null)
            {
                throw new Exception("Users not exists");
            }

            return users;
        }

        public async Task<IEnumerable<User>> GetUsersByIdAsync(IEnumerable<string> usersId)
        {
            var usersResult = new List<User>();
            foreach (var userId in usersId)
            {
                usersResult.Add(await _unitOfWork.UserRepository.GetByIdAsync(userId));
            }

            if (usersResult == null)
            {
                throw new Exception("Users not exists");
            }

            return usersResult;
        }
        #endregion

        #region UserFaceRepository
        public async Task<UserFace> GetUserFaceByIdAsync(string userId, int faceId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new Exception("User not exists");
            }
            var userFace = await _unitOfWork.UserFaceRepository.GetByIdAsync(faceId);

            if (userFace == null || userFace.UserId != user.Id)
            {
                throw new Exception("User face not exists");
            }

            return userFace;
        }

        public async Task<IEnumerable<UserFace>> GetAllUserFacesAsync(string userId)
        {
            var facesResult = await _unitOfWork.UserFaceRepository.GetAllUserFacesAsync(userId);
            
            if (facesResult == null)
            {
                throw new Exception("User faces not exists");
            }

            return facesResult;
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
                throw new Exception("User faces not exists");
            }

            return userFaces;
        }
        #endregion    
    }
}
