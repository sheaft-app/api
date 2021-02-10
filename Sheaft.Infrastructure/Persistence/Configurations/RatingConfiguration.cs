using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("ProductUid");
            entity.Property<long>("UserUid");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();

            entity.Property(c => c.Value).IsRequired().HasColumnType("decimal(10,2)");

            entity.HasOne(c => c.User).WithMany().HasForeignKey("UserUid").OnDelete(DeleteBehavior.NoAction);

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex("ProductUid");
            entity.HasIndex("UserUid");
            entity.HasIndex("Uid", "Id", "ProductUid", "UserUid", "RemovedOn");

            entity.ToTable("Ratings");
        }
    }
}
