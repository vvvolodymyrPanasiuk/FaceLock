using FaceLock.Domain.Entities.DoorLockAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FaceLock.EF.MySql.EntityConfigurations.DoorLockAggregateConfiguration
{
    public class DoorLockEntityConfiguration : IEntityTypeConfiguration<DoorLock>
    {
        public void Configure(EntityTypeBuilder<DoorLock> builder)
        {
            builder.ToTable("DoorLocks");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasMaxLength(256)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(e => e.Name)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(e => e.Description)
                .HasMaxLength(256);

            builder.HasMany(e => e.DoorLockAccesses)
                .WithOne(e => e.DoorLock)
                .HasForeignKey(e => e.DoorLockId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.DoorLockHistories)
                .WithOne(e => e.DoorLock)
                .HasForeignKey(e => e.DoorLockId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.DoorLockAccessTokens)
                .WithOne(e => e.DoorLock)
                .HasForeignKey(e => e.DoorLockId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
