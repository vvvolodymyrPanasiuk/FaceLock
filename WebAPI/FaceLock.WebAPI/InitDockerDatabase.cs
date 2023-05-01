using FaceLock.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace FaceLock.WebAPI
{
    public static class InitDockerDatabase
    {
        public static void Init(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                serviceScope.ServiceProvider.GetService<FaceLockDbContext>().Database.Migrate();

                var server = Environment.GetEnvironmentVariable("DatabaseServer");
                var port = Environment.GetEnvironmentVariable("DatabasePort");
                var user = Environment.GetEnvironmentVariable("DatabaseUser");
                var password = Environment.GetEnvironmentVariable("DatabasePassword");
                var database = Environment.GetEnvironmentVariable("DatabaseName");

                string connectionString = $"Server={server}, {port}; Initial Catalog={database}; User ID={user}; Password={password};TrustServerCertificate=true;";
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    var script = File.ReadAllText("./scripts/token-db-init.sql");
                    var commands = script.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var command in commands)
                    {
                        using (var sqlCommand = new SqlCommand(command, connection))
                        {
                            sqlCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }
}
