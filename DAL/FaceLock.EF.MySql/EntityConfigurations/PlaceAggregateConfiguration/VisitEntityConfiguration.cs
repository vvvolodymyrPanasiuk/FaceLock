using FaceLock.Domain.Entities.PlaceAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FaceLock.EF.MySql.EntityConfigurations.PlaceAggregateConfiguration
{
    public class VisitEntityConfiguration : IEntityTypeConfiguration<Visit>
    {
        public void Configure(EntityTypeBuilder<Visit> builder)
        {
            builder.ToTable("Visits");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.HasIndex(v => v.UserId);

            builder.Property(x => x.PlaceId)
                .IsRequired();

            builder.HasIndex(v => v.PlaceId);

            builder.Property(x => x.CheckInTime)
                .IsRequired()
                .HasDefaultValue(DateTime.Now);

            builder.HasIndex(v => v.CheckInTime);

            builder.Property(x => x.CheckOutTime)
                .IsRequired(false);

            builder.HasOne(v => v.Place)
                   .WithMany(p => p.Visits)
                   .HasForeignKey(v => v.PlaceId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(v => v.User)
                   .WithMany(u => u.Visits)
                   .HasForeignKey(v => v.UserId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
