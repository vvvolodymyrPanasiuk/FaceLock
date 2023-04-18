using FaceLock.Domain.Entities.PlaceAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FaceLock.EF.EntityConfigurations.PlaceAggregateConfiguration
{
    public class PlaceEntityConfiguration : IEntityTypeConfiguration<Place>
    {
        public void Configure(EntityTypeBuilder<Place> builder)
        {
            builder.ToTable("Places");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.HasMany(x => x.Visits)
                .WithOne(y => y.Place)
                .HasForeignKey(y => y.PlaceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
