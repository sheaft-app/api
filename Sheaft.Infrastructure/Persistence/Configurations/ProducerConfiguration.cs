using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class ProducerConfiguration : IEntityTypeConfiguration<Producer>
    {
        public void Configure(EntityTypeBuilder<Producer> entity)
        {
            entity.HasMany(c => c.Tags).WithOne().HasForeignKey("ProducerUid").OnDelete(DeleteBehavior.Cascade);

            var businessTags = entity.Metadata.FindNavigation(nameof(Producer.Tags));
            businessTags.SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.HasMany<DeliveryMode>().WithOne(c => c.Producer).HasForeignKey("ProducerUid").OnDelete(DeleteBehavior.Cascade);
            entity.HasMany<Returnable>().WithOne(c => c.Producer).HasForeignKey("ProducerUid").OnDelete(DeleteBehavior.Cascade);
            
            entity.Ignore(c => c.DomainEvents);
        }
    }
}
