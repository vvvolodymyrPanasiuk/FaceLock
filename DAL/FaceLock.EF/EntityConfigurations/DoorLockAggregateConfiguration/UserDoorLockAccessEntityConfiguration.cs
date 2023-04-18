using FaceLock.Domain.Entities.DoorLockAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FaceLock.EF.EntityConfigurations.DoorLockAggregateConfiguration
{
    public class UserDoorLockAccessEntityConfiguration : IEntityTypeConfiguration<UserDoorLockAccess>
    {
        public void Configure(EntityTypeBuilder<UserDoorLockAccess> builder)
        {
            builder.ToTable("UserDoorLockAccesses");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(e => e.UserId)
                .IsRequired();

            builder.Property(e => e.DoorLockId)
                .IsRequired();

            builder.HasIndex(e => e.UserId);

            builder.HasIndex(e => e.DoorLockId);

            builder.HasOne(e => e.User)
                .WithMany(e => e.DoorLockAccesses)
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.DoorLock)
                .WithMany(e => e.DoorLockAccesses)
                .HasForeignKey(e => e.DoorLockId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}