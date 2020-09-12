using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure
{
    public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> entity)
        {
            entity.Property(c => c.OwnerName).IsRequired();
            entity.Property(c => c.IBAN).IsRequired();
            entity.Property(c => c.BIC).IsRequired();

            entity.OwnsOne(c => c.OwnerAddress, cb =>
            {
                cb.ToTable("BankAccountOwnerAddresses");
            });
        }
    }
}
