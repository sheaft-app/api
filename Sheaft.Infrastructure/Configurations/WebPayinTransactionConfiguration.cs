using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure
{
    public class WebPayinTransactionConfiguration : IEntityTypeConfiguration<WebPayinTransaction>
    {
        public void Configure(EntityTypeBuilder<WebPayinTransaction> entity)
        {
        }
    }
}
