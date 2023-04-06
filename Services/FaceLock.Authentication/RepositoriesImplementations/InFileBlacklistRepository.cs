using FaceLock.Authentication.Repositories;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace FaceLock.Authentication.RepositoriesImplementations
{
    public class InFileBlacklistRepository : IBlacklistRepository
    {
        private readonly string _filePath;
        private readonly IConfiguration _configuration;

        public InFileBlacklistRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _filePath = _configuration["BlacklistFilePath"];
            //_filePath = "blacklist.json";
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
                    return JsonConvert.DeserializeObject<List<RefreshToken>>(json);
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
    }
}
