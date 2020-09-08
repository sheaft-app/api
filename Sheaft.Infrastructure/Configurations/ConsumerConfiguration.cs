using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure
{
    public class ConsumerConfiguration : IEntityTypeConfiguration<Consumer>
    {
        public void Configure(EntityTypeBuilder<Consumer> entity)
        {
        }
    }
}
