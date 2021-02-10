using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class ConsumerLegalConfiguration : IEntityTypeConfiguration<ConsumerLegal>
    {
        public void Configure(EntityTypeBuilder<ConsumerLegal> entity)
        {
        }
    }
}
