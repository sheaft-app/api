using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure
{
    public class PurchaseOrderSenderConfiguration : IEntityTypeConfiguration<PurchaseOrderSender>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrderSender> entity)
        {
            entity.Property<long>("Uid");

            entity.Property(c => c.Name).IsRequired();
            entity.Property(c => c.Email).IsRequired();

            entity.HasKey("Uid");
            entity.HasIndex("Id");

            entity.ToTable("PurchaseOrderSenders");
        }
    }
}
