using FaceLock.Domain.Entities.DoorLockAggregate;
using FaceLock.Domain.Entities.PlaceAggregate;
using FaceLock.Domain.Entities.UserAggregate;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace FaceLock.EF
{
    public class FaceLockDbContext : IdentityDbContext<User>
    {
        public FaceLockDbContext(DbContextOptions<FaceLockDbContext> options) : base(options){}
        
        public override DbSet<User> Users { get; set; }
        public DbSet<UserFace> UserFaces { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<DoorLock> DoorLocks { get; set; }
        public DbSet<UserDoorLockAccess> UserDoorLockAccesses { get; set; }
        public DbSet<DoorLockHistory> DoorLockHistories { get; set; }
        public DbSet<DoorLockAccessToken> DoorLockAccessTokens { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            //optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message), new[] { RelationalEventId.CommandExecuted });
            //optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
