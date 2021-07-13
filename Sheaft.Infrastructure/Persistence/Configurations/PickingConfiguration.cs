using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class PickingConfiguration : IEntityTypeConfiguration<Picking>
    {
        public PickingConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<Picking> entity)
        {
            entity.Property(c => c.RowVersion).IsRowVersion();
            entity.Ignore(c => c.DomainEvents);
            
            entity.HasMany(c => c.PurchaseOrders).WithOne(c => c.Picking)
                .HasForeignKey(c => c.PickingId).OnDelete(DeleteBehavior.SetNull);
            
            entity.HasMany(c => c.PreparedProducts).WithOne()
                .HasForeignKey(c => c.PickingId).OnDelete(DeleteBehavior.Cascade);
            
            entity.HasMany(c => c.ProductsToPrepare).WithOne()
                .HasForeignKey(c => c.PickingId).OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(c => c.Producer).WithMany()
                .HasForeignKey(c => c.ProducerId).OnDelete(DeleteBehavior.NoAction);
            
            entity.HasKey(c => c.Id);
            entity.ToTable("Pickings");
        }
    }
}