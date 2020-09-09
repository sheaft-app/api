using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Interop.Enums;

namespace Sheaft.Infrastructure
{
    public class TransferPaymentConfiguration : IEntityTypeConfiguration<TransferPayment>
    {
        public void Configure(EntityTypeBuilder<TransferPayment> entity)
        {
            entity.Property(c => c.IBAN).IsRequired();
            entity.Property(c => c.BIC).IsRequired();
        }
    }
}
