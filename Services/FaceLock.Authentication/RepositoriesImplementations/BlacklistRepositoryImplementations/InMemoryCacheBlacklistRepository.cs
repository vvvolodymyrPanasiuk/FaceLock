using FaceLock.Authentication.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace FaceLock.Authentication.RepositoriesImplementations.BlacklistRepositoryImplementations
{
    /// <summary>
    /// Implementation of IBlacklistRepository that uses IMemoryCache as an in-memory cache to store blacklisted tokens.
    /// </summary>
    public class InMemoryCacheBlacklistRepository : IBlacklistRepository
    {
        private readonly IMemoryCache _cache;

        public InMemoryCacheBlacklistRepository(IMemoryCache cache)
        {
            _cache = cache;
        }


        public async Task<bool> AddTokenToBlacklistAsync(string refreshToken, TimeSpan expirationTime)
        {
            _cache.Set(refreshToken, true, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expirationTime
            }.SetSize(1));

            return await IsTokenInBlacklistAsync(refreshToken);
        }

        public async Task<bool> IsTokenInBlacklistAsync(string refreshToken)
        {
            return await Task.FromResult(_cache.TryGetValue(refreshToken, out _));
        }
    }
}
