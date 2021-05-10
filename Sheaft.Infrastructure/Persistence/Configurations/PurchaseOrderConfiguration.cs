using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
    {
        private readonly bool _isAdmin;

        public PurchaseOrderConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }

        public void Configure(EntityTypeBuilder<PurchaseOrder> entity)
        {
            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn);
            entity.Property(c => c.RowVersion).IsRowVersion();

            if (!_isAdmin)
                entity.HasQueryFilter(p => !p.RemovedOn.HasValue);

            entity.Property(o => o.Reference).IsRequired();
            entity.Property(o => o.DroppedOn).HasColumnName("WithdrawnOn");

            entity.Property(o => o.TotalWeight).HasColumnType("decimal(10,2)");

            entity.Property(o => o.TotalWholeSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalVatPrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalOnSalePrice).HasColumnType("decimal(10,2)");

            entity.Property(o => o.TotalReturnableOnSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalReturnableVatPrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalReturnableWholeSalePrice).HasColumnType("decimal(10,2)");

            entity.Property(o => o.TotalProductOnSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalProductVatPrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalProductWholeSalePrice).HasColumnType("decimal(10,2)");

            entity.HasMany(o => o.Products).WithOne().HasForeignKey(c => c.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade).IsRequired();

            entity.OwnsOne(c => c.VendorInfo, c =>
            {
                c.Property(e => e.Name).UseCollation("Latin1_general_CI_AI").IsRequired();
                c.Property(e => e.Email).IsRequired();
            });
            entity.OwnsOne(c => c.SenderInfo, c =>
            {
                c.Property(e => e.Name).UseCollation("Latin1_general_CI_AI").IsRequired();
                c.Property(e => e.Email).IsRequired();
            });
            entity.OwnsOne(c => c.ExpectedDelivery, cb => { cb.OwnsOne(ca => ca.Address); });

            entity.Ignore(c => c.DomainEvents);

            entity.HasKey(c => c.Id);
            entity.HasIndex(c => new {c.ProducerId, c.Reference}).IsUnique();
            entity.ToTable("PurchaseOrders");
        }
    }
}