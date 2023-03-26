using FaceLock.Domain.Entities.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FaceLock.EF.EntityConfigurations
{
    public class FaceUserEntityConfiguration : IEntityTypeConfiguration<UserFace>
    {
        public void Configure(EntityTypeBuilder<UserFace> builder)
        {
            builder.ToTable("FaceUsers");

            builder.HasKey(f => f.Id);

            builder.Property(f => f.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(f => f.ImageData)
                .IsRequired();

            builder.Property(f => f.ImageMimeType)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(f => f.User)
                .WithMany(u => u.UserFaces)
                .HasForeignKey(f => f.UserId)
                .IsRequired();
        }
    }
}
