using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class PayinRefundConfiguration : IEntityTypeConfiguration<PayinRefund>
    {
        public void Configure(EntityTypeBuilder<PayinRefund> entity)
        {
            entity.Property<long>("PayinUid");
            entity.HasOne(c => c.Payin).WithMany().HasForeignKey("PayinUid").OnDelete(DeleteBehavior.NoAction);
            entity.HasIndex("PayinUid");
        }
    }
}
