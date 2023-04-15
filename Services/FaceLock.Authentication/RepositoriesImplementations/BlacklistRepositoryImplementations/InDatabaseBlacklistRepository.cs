using FaceLock.Authentication.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace FaceLock.Authentication.RepositoriesImplementations.BlacklistRepositoryImplementations
{
    /// <summary>
    /// Implementation of IBlacklistRepository that stores blacklisted tokens in a SQL database.
    /// </summary>
    public class InDatabaseBlacklistRepository : IBlacklistRepository
    {
        private readonly string _connectionString;

        public InDatabaseBlacklistRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("BlacklistConnection");
        }


        public async Task<bool> AddTokenToBlacklistAsync(string refreshToken, TimeSpan expirationTime)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "INSERT INTO BlacklistTokens(Token, ExpirationTime) VALUES(@token, @expirationTime)";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@token", refreshToken);
                command.Parameters.AddWithValue("@expirationTime", DateTime.UtcNow.Add(expirationTime));
                await command.ExecuteNonQueryAsync();
                await RemoveExpiredTokensFromBlacklistAsync();

                return await IsTokenInBlacklistAsync(refreshToken);
            }
        }

        public async Task<bool> IsTokenInBlacklistAsync(string refreshToken)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT COUNT(*) FROM BlacklistTokens WHERE Token = @token AND ExpirationTime > @currentTime";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@token", refreshToken);
                command.Parameters.AddWithValue("@currentTime", DateTime.UtcNow);
                var result = await command.ExecuteScalarAsync() as int?;

                if (result == null)
                {
                    throw new ApplicationException("Failed to check if token is in blacklist.");
                }
                return result > 0;
            }
        }

        private async Task RemoveExpiredTokensFromBlacklistAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "DELETE FROM BlacklistTokens WHERE ExpirationTime <= @currentTime";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@currentTime", DateTime.UtcNow);
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
