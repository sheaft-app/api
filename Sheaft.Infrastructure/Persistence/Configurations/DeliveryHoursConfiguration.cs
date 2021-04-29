using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class DeliveryHoursConfiguration : IEntityTypeConfiguration<DeliveryHours>
    {
        public void Configure(EntityTypeBuilder<DeliveryHours> entity)
        {
            entity.HasKey(c => c.Id);
            
            entity.ToTable("DeliveryHours");
        }
    }
}