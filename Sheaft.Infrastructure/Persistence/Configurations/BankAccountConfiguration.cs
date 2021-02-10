using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> entity)
        {
            entity.Property(c => c.Owner).IsRequired();
            entity.Property(c => c.IBAN).IsRequired();
            entity.Property(c => c.BIC).IsRequired();
            entity.Property(c => c.BIC).IsRequired();
        }
    }
}
