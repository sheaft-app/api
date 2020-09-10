using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure
{
    public class TransferConfiguration : IEntityTypeConfiguration<Transfer>
    {
        public void Configure(EntityTypeBuilder<Transfer> entity)
        {
            entity.Property(c => c.OwnerName).IsRequired();
            entity.Property(c => c.IBAN).IsRequired();
            entity.Property(c => c.BIC).IsRequired();

            entity.Property(c => c.IsActive).HasDefaultValue(true);

            entity.OwnsOne(c => c.OwnerAddress, cb =>
            {
                cb.ToTable("TransferOwnerAddresses");
            });
        }
    }
}
