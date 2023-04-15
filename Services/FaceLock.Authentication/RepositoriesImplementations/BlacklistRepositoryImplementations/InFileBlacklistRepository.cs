using FaceLock.Authentication.Repositories;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace FaceLock.Authentication.RepositoriesImplementations.BlacklistRepositoryImplementations
{
    /// <summary>
    /// Implementation of IBlacklistRepository that stores blacklisted tokens in a file(JSON).
    /// </summary>
    public class InFileBlacklistRepository : IBlacklistRepository
    {
        private readonly string _filePath;

        public InFileBlacklistRepository(IConfiguration configuration)
        {
            _filePath = configuration["BlacklistFilePath"];
        }


        public async Task<bool> AddTokenToBlacklistAsync(string refreshToken, TimeSpan expirationTime)
        {
            var blacklist = await LoadBlacklistAsync();
            blacklist.Add(new RefreshToken
            {
                Token = refreshToken,
                RefreshTokenExpires = DateTime.UtcNow.AddMinutes(expirationTime.TotalMinutes),
            });
            await SaveBlacklistAsync(blacklist);
            await RemoveExpiredTokensFromBlacklistAsync();

            return await IsTokenInBlacklistAsync(refreshToken);
        }

        public async Task<bool> IsTokenInBlacklistAsync(string token)
        {
            var blacklist = await LoadBlacklistAsync();
            return blacklist.Any(t => t.Token == token);
        }

        private async Task<List<RefreshToken>> LoadBlacklistAsync()
        {
            if (!File.Exists(_filePath))
            {
                return new List<RefreshToken>();
            }

            using (var fileStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read))
            {
                using (var streamReader = new StreamReader(fileStream))
                {
                    var json = await streamReader.ReadToEndAsync();
                    var result = JsonConvert.DeserializeObject<List<RefreshToken>>(json);

                    if (result is null)
                    {
                        throw new ApplicationException("Failed loading tokens from blacklist.");
                    }
                    return result;
                }
            }
        }

        private async Task SaveBlacklistAsync(List<RefreshToken> blacklist)
        {
            using (var fileStream = new FileStream(_filePath, FileMode.Create, FileAccess.Write))
            {
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    var json = JsonConvert.SerializeObject(blacklist);
                    await streamWriter.WriteAsync(json);
                }
            }
        }

        private async Task RemoveExpiredTokensFromBlacklistAsync()
        {
            var blacklist = await LoadBlacklistAsync();
            var currentTime = DateTime.UtcNow;
            blacklist.RemoveAll(t => t.RefreshTokenExpires <= currentTime);
            await SaveBlacklistAsync(blacklist);
        }
    }
}
