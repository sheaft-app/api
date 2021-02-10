using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class WebPayinConfiguration : IEntityTypeConfiguration<WebPayin>
    {
        public void Configure(EntityTypeBuilder<WebPayin> entity)
        {
        }
    }
}
