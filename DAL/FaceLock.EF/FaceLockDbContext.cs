using FaceLock.Domain.Entities.PlaceAggregate;
using FaceLock.Domain.Entities.UserAggregate;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace FaceLock.EF
{
    public class FaceLockDbContext : IdentityDbContext<User>
    {
        public FaceLockDbContext()
        {
            //Database.EnsureCreated();
        }

        public FaceLockDbContext(DbContextOptions<FaceLockDbContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }
        
        public override DbSet<User> Users { get; set; }
        public DbSet<UserFace> UserFaces { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Visit> Visits { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message), new[] { RelationalEventId.CommandExecuted });
            //optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.UserFaces)
                .WithOne(uf => uf.User)
                .HasForeignKey(uf => uf.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserFace>()
                .HasOne(uf => uf.User)
                .WithMany(u => u.UserFaces)
                .HasForeignKey(uf => uf.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Visit>()
                .HasOne(v => v.User)
                .WithMany(u => u.Visits)
                .HasForeignKey(v => v.UserId)
                .IsRequired().
                OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Visit>()
                .HasOne(v => v.Place)
                .WithMany(r => r.Visits)
                .HasForeignKey(v => v.PlaceId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
