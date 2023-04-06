using FaceLock.Authentication.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace FaceLock.Authentication.RepositoriesImplementations
{
    public class InDatabaseBlacklistRepository : IBlacklistRepository
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public InDatabaseBlacklistRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("BlacklistConnection");
        }


        public async Task<bool> AddTokenToBlacklistAsync(string token, TimeSpan expirationTime)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "INSERT INTO BlacklistTokens(Token, ExpirationTime) VALUES(@token, @expirationTime)";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@token", token);
                command.Parameters.AddWithValue("@expirationTime", DateTime.UtcNow.Add(expirationTime));
                await command.ExecuteNonQueryAsync();
                return await IsTokenInBlacklistAsync(token);
            }
        }

        public async Task<bool> IsTokenInBlacklistAsync(string token)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT COUNT(*) FROM BlacklistTokens WHERE Token = @token AND ExpirationTime > @currentTime";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@token", token);
                command.Parameters.AddWithValue("@currentTime", DateTime.UtcNow);
                var result = (int)await command.ExecuteScalarAsync();

                return result > 0;
            }
        }

        public async Task RemoveExpiredTokensFromBlacklistAsync()
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
        //Створити новий файл з назвою DeleteExpiredTokens.sql з наступним вмістом
        //DELETE FROM BlacklistTokens WHERE ExpirationDate <= GETUTCDATE();
        public async Task DeleteExpiredTokens()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("EXEC sp_start_job 'DeleteExpiredTokens';", connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
