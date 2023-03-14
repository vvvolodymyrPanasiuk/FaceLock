using FaceLock.Domain.Entities.RoomAggregate;
using FaceLock.Domain.Entities.UserAggregate;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Visit> Visits { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MoneyDB;Trusted_Connection=True;");
            //optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message), new[] { RelationalEventId.CommandExecuted });

            //optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Конфігурація зв'язку один-до-багатьох між User і UserFace
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

            // Конфігурація зв'язку багатьох-до-багатьох між User і Visit
            //???????????????
            /*modelBuilder.Entity<Visit>()
                .HasKey(v => new { v.UserId, v.RoomId });
            */
            modelBuilder.Entity<Visit>()
                .HasOne(v => v.User)
                .WithMany(u => u.Visits)
                .HasForeignKey(v => v.UserId)
                .IsRequired().
                OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Visit>()
                .HasOne(v => v.Room)
                .WithMany(r => r.Visits)
                .HasForeignKey(v => v.RoomId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            


            /*
            modelBuilder.Entity<User>().HasMany(b => b.DebtAccounts).WithOne().HasForeignKey(k => k.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>().HasMany(b => b.SavingAccounts).WithOne().HasForeignKey(k => k.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>().HasMany(b => b.RegularAccounts).WithOne().HasForeignKey(k => k.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>().HasMany(b => b.Categories).WithOne().HasForeignKey(k => k.UserId).OnDelete(DeleteBehavior.Cascade);
            */

            /*
            modelBuilder.Entity<Category>().HasMany(b => b.Subcategories).WithOne().HasForeignKey(k => k.CategoryId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Category>().HasMany(b => b.DefaultTransactions).WithOne().HasForeignKey(k => k.CategoryId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<DebtAccount>().HasMany(b => b.DebtAccountTransactions).WithOne().HasForeignKey(k => k.DebtAccountId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<SavingAccount>().HasMany(b => b.SavingAccountTransactions).WithOne().HasForeignKey(k => k.SavingAccountId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<RegularAccount>().HasMany(b => b.RegularAccountTransactions).WithOne().HasForeignKey(k => k.RegularAccountId).OnDelete(DeleteBehavior.Cascade);
            */

            #region Required
            /*
            modelBuilder.Entity<Subcategory>().Property(b => b.Name).IsRequired();
            modelBuilder.Entity<SavingAccount>().Property(b => b.Name).IsRequired();
            modelBuilder.Entity<RegularAccount>().Property(b => b.Name).IsRequired();
            modelBuilder.Entity<DebtAccount>().Property(b => b.Name).IsRequired();
            modelBuilder.Entity<Category>().Property(b => b.Name).IsRequired();
            */
            #endregion

            #region OwnsMany
            //modelBuilder.Entity<User>().OwnsMany(u => u.DebtAccounts);
            //modelBuilder.Entity<User>().OwnsMany(u => u.SavingAccounts);
            //modelBuilder.Entity<User>().OwnsMany(u => u.RegularAccounts);
            //modelBuilder.Entity<User>().OwnsMany(u => u.Categories);
            //modelBuilder.Entity<Category>().OwnsMany(u => u.Subcategories);
            //modelBuilder.Entity<Category>().OwnsMany(u => u.DefaultTransactions);
            //modelBuilder.Entity<DebtAccount>().OwnsMany(u => u.DebtAccountTransactions);
            //modelBuilder.Entity<SavingAccount>().OwnsMany(u => u.SavingAccountTransactions);
            //modelBuilder.Entity<RegularAccount>().OwnsMany(u => u.RegularAccountTransactions);
            #endregion

            //Use IgnoreQueryFilters() for ignore filter (exemple: db.Users.IgnoreQueryFilters().Where(x => x.Login == login && x.Password == password);
            //modelBuilder.Entity<User>().HasQueryFilter(u => u.IsEmailConfirmed == true);

            //modelBuilder.Entity<User>().Property(u => u.IsEmailConfirmed).HasDefaultValue(false);

            #region HasDefaultValueSql
            //modelBuilder.Entity<DebtAccountTransaction>().Property(u => u.Date).HasDefaultValueSql("GETDATE()");
            //modelBuilder.Entity<DefaultTransaction>().Property(u => u.Date).HasDefaultValueSql("GETDATE()");
            //modelBuilder.Entity<RegularAccountTransaction>().Property(u => u.Date).HasDefaultValueSql("GETDATE()");
            //modelBuilder.Entity<SavingAccountTransaction>().Property(u => u.Date).HasDefaultValueSql("GETDATE()");
            #endregion

            #region Index
            //modelBuilder.Entity<User>().HasIndex(u => new { u.Login, u.Email });
            //modelBuilder.Entity<DebtAccount>().HasIndex(u => u.UserId);
            //modelBuilder.Entity<RegularAccount>().HasIndex(u => u.UserId);
            //modelBuilder.Entity<SavingAccount>().HasIndex(u => u.UserId);
            //modelBuilder.Entity<Category>().HasIndex(u => u.UserId);
            //modelBuilder.Entity<Subcategory>().HasIndex(u => u.CategoryId);
            //modelBuilder.Entity<DebtAccountTransaction>().HasIndex(u => u.DebtAccountId);
            //modelBuilder.Entity<DefaultTransaction>().HasIndex(u => u.CategoryId);
            //modelBuilder.Entity<RegularAccountTransaction>().HasIndex(u => u.RegularAccountId);
            //modelBuilder.Entity<SavingAccountTransaction>().HasIndex(u => u.SavingAccountId);
            #endregion

            #region HasMaxLength
            //modelBuilder.Entity<User>().Property(b => b.Login).HasMaxLength(100);
            //modelBuilder.Entity<User>().Property(b => b.Email).HasMaxLength(50);
            //modelBuilder.Entity<User>().Property(b => b.Name).HasMaxLength(50);
            #endregion
            base.OnModelCreating(modelBuilder);
        }
    }
}
