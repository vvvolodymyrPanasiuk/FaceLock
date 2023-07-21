using FaceLock.Authentication.Repositories;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace FaceLock.Authentication.RepositoriesImplementations.TokenStateRepositoryImplementations
{
    public class InFileTokenStateRepository : ITokenStateRepository
    {
        private readonly string _filePath;

        public InFileTokenStateRepository(IConfiguration configuration)
        {
            _filePath = configuration["TokenStateFilePath"];
        }

        public async Task<List<RefreshToken>> GetRefreshTokenByUserIdAsync(string userId)
        {
            var tokenStates = await ReadTokenStatesFromFileAsync();
            return tokenStates.FindAll(tokenState => tokenState.UserId == userId);
        }

        public async Task<RefreshToken> GetRefreshTokenAsync(string refreshToken)
        {
            var tokenStates = await ReadTokenStatesFromFileAsync();
            return tokenStates.Find(tokenState => tokenState.Token == refreshToken);
        }

        public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
        {
            var tokenStates = await ReadTokenStatesFromFileAsync();
            tokenStates.Add(refreshToken);
            await WriteTokenStatesToFileAsync(tokenStates);
        }

        public async Task<bool> IsRefreshTokenValidAsync(string refreshToken)
        {
            var tokenStates = await ReadTokenStatesFromFileAsync();
            var tokenState = tokenStates.Find(tokenState => tokenState.Token == refreshToken);
            return tokenState != null && tokenState.RefreshTokenExpires > DateTime.UtcNow;
        }

        public async Task RemoveRefreshTokenAsync(string refreshToken)
        {
            var tokenStates = await ReadTokenStatesFromFileAsync();
            tokenStates.RemoveAll(tokenState => tokenState.Token == refreshToken);
            await WriteTokenStatesToFileAsync(tokenStates);
        }

        private async Task<List<RefreshToken>> ReadTokenStatesFromFileAsync()
        {
            if (!File.Exists(_filePath))
            {
                return new List<RefreshToken>();
            }

            var jsonString = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<RefreshToken>>(jsonString);
        }

        private async Task WriteTokenStatesToFileAsync(List<RefreshToken> tokenStates)
        {
            var jsonString = JsonSerializer.Serialize(tokenStates, new JsonSerializerOptions
            {
                WriteIndented = true // For pretty
            });

            await File.WriteAllTextAsync(_filePath, jsonString);
        }
    }
}
