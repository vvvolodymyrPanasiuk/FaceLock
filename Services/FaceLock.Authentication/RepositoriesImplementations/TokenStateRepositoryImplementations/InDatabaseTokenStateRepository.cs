using FaceLock.Authentication.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace FaceLock.Authentication.RepositoriesImplementations.TokenStateRepositoryImplementations
{
    /// <summary>
    /// Implementation of ITokenStateRepository that stores token state information in a SQL database.
    /// </summary>
    public class InDatabaseTokenStateRepository : ITokenStateRepository
    {
        private readonly string _connectionString;

        public InDatabaseTokenStateRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("TokenStateConnection");
        }


        public async Task<List<RefreshToken>> GetRefreshTokenByUserIdAsync(string userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                var query = "SELECT * FROM TokenStates WHERE UserId = @userId";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var refreshTokenList = new List<RefreshToken>();
                        while (await reader.ReadAsync())
                        {
                            var refreshToken = new RefreshToken
                            {
                                Token = reader["Token"].ToString(),
                                UserId = reader["UserId"].ToString(),
                                RefreshTokenExpires = Convert.ToDateTime(reader["RefreshTokenExpires"]),
                                Device = reader["Device"].ToString(),
                                Country = reader["Country"].ToString(),
                                City = reader["City"].ToString(),
                                TimeCreated = Convert.ToDateTime(reader["TimeCreated"])
                            };

                            refreshTokenList.Add(refreshToken);
                        }

                        return refreshTokenList ?? new List<RefreshToken>();
                    }
                }
            }
        }

        public async Task<RefreshToken> GetRefreshTokenAsync(string refreshToken)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                var query = "SELECT * FROM TokenStates WHERE Token = @token";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@token", refreshToken);
                    
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var refreshTokenObject = new RefreshToken
                            {
                                Token = reader["Token"].ToString(),
                                UserId = reader["UserId"].ToString(),
                                RefreshTokenExpires = Convert.ToDateTime(reader["RefreshTokenExpires"]),
                                Device = reader["Device"].ToString(),
                                Country = reader["Country"].ToString(),
                                City = reader["City"].ToString(),
                                TimeCreated = Convert.ToDateTime(reader["TimeCreated"])
                            };

                            return refreshTokenObject;
                        }

                        return new RefreshToken();
                    }
                }
            }
        }

        public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                var query = "INSERT INTO TokenStates (Token, UserId, RefreshTokenExpires) " +
                            "VALUES (@token, @userId, @expires, @country, @city, @device, @timeCreated)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@token", refreshToken.Token);
                    command.Parameters.AddWithValue("@userId", refreshToken.UserId);
                    command.Parameters.AddWithValue("@expires", refreshToken.RefreshTokenExpires);
                    command.Parameters.AddWithValue("@country", refreshToken.Country);
                    command.Parameters.AddWithValue("@city", refreshToken.City);
                    command.Parameters.AddWithValue("@device", refreshToken.Device);
                    command.Parameters.AddWithValue("@timeCreated", refreshToken.TimeCreated);

                    await RemoveExpiredTokensFromTokenStateAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<bool> IsRefreshTokenValidAsync(string refreshToken)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                var query = "SELECT COUNT(*) FROM TokenStates WHERE Token = @token AND RefreshTokenExpires > @currentDateTime";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@token", refreshToken);
                    command.Parameters.AddWithValue("@currentDateTime", DateTime.UtcNow);
                    
                    var result = await command.ExecuteScalarAsync() as int?;
                    if (result == null)
                    {
                        throw new ApplicationException("Failed to check if token is in tokenStateDB.");
                    }

                    return result > 0;
                }
            }
        }

        public async Task RemoveRefreshTokenAsync(string refreshToken)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                var query = "DELETE FROM TokenStates WHERE Token = @token";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@token", refreshToken);
                    
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        private async Task RemoveExpiredTokensFromTokenStateAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "DELETE FROM TokenStates WHERE RefreshTokenExpires <= @currentTime";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@currentTime", DateTime.UtcNow);
                
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}