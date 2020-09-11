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
            entity.Property<long?>("CardUid");

            entity.HasOne(c => c.Card).WithMany().HasForeignKey("CardUid").OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(c => c.Order).WithMany().HasForeignKey("OrderUid").OnDelete(DeleteBehavior.NoAction);

            entity.HasIndex("OrderUid");
            entity.HasIndex("CardUid");
        }
    }
}
