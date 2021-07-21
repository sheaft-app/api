using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class ObservationProductConfiguration : IEntityTypeConfiguration<ObservationProduct>
    {
        public void Configure(EntityTypeBuilder<ObservationProduct> entity)
        {
            entity.Property(c => c.CreatedOn);

            entity.HasOne(c => c.Product).WithMany().HasForeignKey(c => c.ProductId).OnDelete(DeleteBehavior.NoAction);

            entity.HasKey(c => c.Id);
            entity.ToTable("ObservationProducts");
        }
    }
}