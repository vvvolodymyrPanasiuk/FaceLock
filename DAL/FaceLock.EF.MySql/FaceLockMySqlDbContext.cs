using FaceLock.Domain.Entities.DoorLockAggregate;
using FaceLock.Domain.Entities.PlaceAggregate;
using FaceLock.Domain.Entities.UserAggregate;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FaceLock.EF.MySql
{
    public class FaceLockMySqlDbContext : IdentityDbContext<User>
    {
        public FaceLockMySqlDbContext(DbContextOptions<FaceLockMySqlDbContext> options) : base(options) { }

        public override DbSet<User> Users { get; set; }
        public DbSet<UserFace> UserFaces { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<DoorLock> DoorLocks { get; set; }
        public DbSet<UserDoorLockAccess> UserDoorLockAccesses { get; set; }
        public DbSet<DoorLockHistory> DoorLockHistories { get; set; }
        public DbSet<DoorLockSecurityInfo> DoorLockSecurityInformations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
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
