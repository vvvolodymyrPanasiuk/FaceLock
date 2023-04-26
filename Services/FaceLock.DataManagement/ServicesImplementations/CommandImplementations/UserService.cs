using FaceLock.DataManagement.Services.Commands;
using FaceLock.Domain.Entities.UserAggregate;
using FaceLock.Domain.Repositories;
using Microsoft.AspNetCore.Http;

namespace FaceLock.DataManagement.ServicesImplementations.CommandImplementations
{
    public partial class UserService : ICommandUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        #region UserRepository
        public async Task AddUserAsync(User user)
        {
            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(User user)
        {
            await _unitOfWork.UserRepository.DeleteAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteUsersAsync(IEnumerable<User> users)
        {
            await _unitOfWork.UserRepository.DeleteRangeAsync(users);
            await _unitOfWork.SaveChangesAsync();
        }
        #endregion

        #region UserFaceRepository
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

        public async Task DeleteUserFaceAsync(UserFace userFace)
        {
            await _unitOfWork.UserFaceRepository.DeleteAsync(userFace);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteUserFacesAsync(IEnumerable<UserFace> userFaces)
        {
            await _unitOfWork.UserFaceRepository.DeleteRangeAsync(userFaces);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddUserFaceAsync(string userId, IFormFile userFace)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new Exception("User not exist");
            }

            if (userFace == null || userFace.Length == 0)
            {
                throw new Exception("File error");
            }

            // Save the user's photo
            using (var memoryStream = new MemoryStream())
            {
                await userFace.CopyToAsync(memoryStream);

                await _unitOfWork.UserFaceRepository
                    .AddAsync(new UserFace
                    {
                        ImageData = memoryStream.ToArray(),
                        ImageMimeType = userFace.ContentType,
                        UserId = user.Id
                    });
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddUserFacesAsync(string userId, IFormFileCollection userFaces)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new Exception("User not exist");
            }

            foreach (var face in userFaces)
            {
                if (face == null || face.Length == 0)
                {
                    throw new Exception("File error");
                }

                // Save the user's photo
                using (var memoryStream = new MemoryStream())
                {
                    await face.CopyToAsync(memoryStream);

                    await _unitOfWork.UserFaceRepository
                        .AddAsync(new UserFace
                        {
                            ImageData = memoryStream.ToArray(),
                            ImageMimeType = face.ContentType,
                            UserId = user.Id
                        });
                }
            }
            await _unitOfWork.SaveChangesAsync();
        }
        #endregion
    }
}
