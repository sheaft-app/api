using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class StoreConfiguration : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> entity)
        {
            entity.HasMany(c => c.OpeningHours).WithOne().HasForeignKey(c => c.StoreId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(c => c.Tags).WithOne().HasForeignKey(c=>c.StoreId).OnDelete(DeleteBehavior.Cascade);

            entity.Ignore(c => c.DomainEvents);
        }
    }
}
