using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class ProducerConfiguration : IEntityTypeConfiguration<Producer>
    {
        public void Configure(EntityTypeBuilder<Producer> entity)
        {
            entity.HasMany(c => c.Tags).WithOne().HasForeignKey(c=>c.ProducerId).OnDelete(DeleteBehavior.Cascade);

            entity.Ignore(c => c.DomainEvents);
        }
    }
}
