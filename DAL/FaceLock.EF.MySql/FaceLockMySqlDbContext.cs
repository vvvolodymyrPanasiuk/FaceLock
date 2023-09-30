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
        public FaceLockMySqlDbContext() { }
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
            string qqqq = "Server=bbttldm6uicnfbpjncx0-mysql.services.clever-cloud.com;Port=3306;Database=bbttldm6uicnfbpjncx0;Persist Security Info=True;User=u182w8veachlcmul;Password=pRm2XXERQeqsuuarcujX;Connection Timeout=60;";
            optionsBuilder.UseMySql(qqqq, new MySqlServerVersion(new Version(5, 1)));
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
