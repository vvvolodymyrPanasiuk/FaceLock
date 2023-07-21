using FaceLock.Domain.Entities.DoorLockAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FaceLock.EF.MySql.EntityConfigurations.DoorLockAggregateConfiguration
{
    public class DoorLockHistoryEntityConfiguration : IEntityTypeConfiguration<DoorLockHistory>
    {
        public void Configure(EntityTypeBuilder<DoorLockHistory> builder)
        {
            builder.ToTable("DoorLockHistories");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(e => e.UserId)
                .IsRequired();

            builder.HasIndex(e => e.UserId);

            builder.Property(e => e.DoorLockId)
                .IsRequired();

            builder.HasIndex(e => e.DoorLockId);

            builder.Property(e => e.OpenedDateTime)
                .IsRequired()
                .HasDefaultValue(DateTime.Now);

            builder.HasIndex(e => e.OpenedDateTime);

            builder.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.DoorLock)
                .WithMany()
                .HasForeignKey(e => e.DoorLockId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
