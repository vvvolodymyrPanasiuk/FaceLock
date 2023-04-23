using FaceLock.DataManagement.Services.Commands;
using FaceLock.Domain.Entities.UserAggregate;
using FaceLock.Domain.Repositories;

namespace FaceLock.DataManagement.ServicesImplementations.CommandImplementations
{
    public partial class UserService : ICommandUserService
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
            foreach (var userFace in userFaces)
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

        public async Task UpdateUserAsync(User user)
        {
            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
