using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> entity)
        {
            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn);
            entity.Property(c => c.RowVersion).IsRowVersion();

            entity.Property(c => c.Value).IsRequired().HasColumnType("decimal(10,2)");

            entity.HasOne(c => c.User).WithMany().HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            entity.HasKey(c => c.Id);
            entity.ToTable("Ratings");
        }
    }
}