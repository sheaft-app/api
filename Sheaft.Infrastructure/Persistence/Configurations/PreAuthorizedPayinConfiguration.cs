using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class PreAuthorizedPayinConfiguration : IEntityTypeConfiguration<PreAuthorizedPayin>
    {
        public void Configure(EntityTypeBuilder<PreAuthorizedPayin> entity)
        {
        }
    }
}