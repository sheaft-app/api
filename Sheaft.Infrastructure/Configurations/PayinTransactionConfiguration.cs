using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure
{
    public class PayinTransactionConfiguration : IEntityTypeConfiguration<PayinTransaction>
    {
        public void Configure(EntityTypeBuilder<PayinTransaction> entity)
        {
            entity.Property<long>("OrderUid");

            entity.HasOne(c => c.Order).WithMany(c => c.Transactions).HasForeignKey("OrderUid").OnDelete(DeleteBehavior.NoAction);

            entity.HasIndex("OrderUid");
        }
    }
}
