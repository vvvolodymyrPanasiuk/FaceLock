using FaceLock.Domain.Entities.DoorLockAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FaceLock.EF.EntityConfigurations.DoorLockAggregateConfiguration
{
    public class DoorLockAccessTokenEntityConfiguration : IEntityTypeConfiguration<DoorLockAccessToken>
    {
        public void Configure(EntityTypeBuilder<DoorLockAccessToken> builder)
        {
            builder.ToTable("DoorLockAccessTokens");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(e => e.DoorLockId)
                .IsRequired();

            builder.HasIndex(e => e.DoorLockId);

            builder.Property(e => e.Utilized)
                .IsRequired()
                .HasDefaultValue(false);

            builder.HasOne(e => e.DoorLock)
                .WithMany()
                .HasForeignKey(e => e.DoorLockId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
