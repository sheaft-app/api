using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure
{
    public class RefundPayinTransactionConfiguration : IEntityTypeConfiguration<RefundPayinTransaction>
    {
        public void Configure(EntityTypeBuilder<RefundPayinTransaction> entity)
        {
            entity.Property<long>("OrderUid");
            
            entity.HasOne(c => c.Order).WithMany().HasForeignKey("OrderUid").OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex("OrderUid");
        }
    }
}
