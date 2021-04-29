using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class OpeningHoursConfiguration : IEntityTypeConfiguration<OpeningHours>
    {
        public void Configure(EntityTypeBuilder<OpeningHours> entity)
        {
            entity.HasKey(c => c.Id);
            
            entity.ToTable("OpeningHours");
        }
    }
}