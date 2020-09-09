using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Interop.Enums;

namespace Sheaft.Infrastructure
{
    public class CardPaymentConfiguration : IEntityTypeConfiguration<CardPayment>
    {
        public void Configure(EntityTypeBuilder<CardPayment> entity)
        {
        }
    }
}
