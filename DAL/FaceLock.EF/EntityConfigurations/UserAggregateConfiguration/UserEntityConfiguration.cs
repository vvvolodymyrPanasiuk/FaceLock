using FaceLock.Domain.Entities.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FaceLock.EF.EntityConfigurations.UserAggregateConfiguration
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasMaxLength(256)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(e => e.UserName)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(e => e.NormalizedUserName)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(e => e.Email)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(e => e.NormalizedEmail)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.Status)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.PasswordHash)
                .IsRequired();

            builder.Property(e => e.SecurityStamp)
                .HasMaxLength(256);

            builder.Property(e => e.ConcurrencyStamp)
                .HasMaxLength(256);

            builder.Property(e => e.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(e => e.PhoneNumberConfirmed)
                .HasDefaultValue(false);

            builder.Property(e => e.TwoFactorEnabled)
                .HasDefaultValue(false);

            builder.Property(e => e.LockoutEnabled)
                .HasDefaultValue(false);

            builder.Property(e => e.AccessFailedCount)
                .HasDefaultValue(0);

            builder.HasMany(e => e.UserFaces)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Visits)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.DoorLockAccesses)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.DoorLockHistories)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
