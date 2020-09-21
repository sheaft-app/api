using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure
{
    public class RefundPayinTransactionConfiguration : IEntityTypeConfiguration<RefundPayinTransaction>
    {
        public void Configure(EntityTypeBuilder<RefundPayinTransaction> entity)
        {
            entity.Property<long>("PayinTransactionUid");
            entity.HasOne(c => c.PayinTransaction).WithMany().HasForeignKey("PayinTransactionUid").OnDelete(DeleteBehavior.NoAction);
            entity.HasIndex("PayinTransactionUid");
        }
    }
}
