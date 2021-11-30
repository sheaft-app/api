using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Database.Configurations
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
            if (!_isAdmin)
                entity.HasQueryFilter(p => !p.Removed);

            entity.Ignore(c => c.DomainEvents);

            entity
                .Property(c => c.UpdatedOn)
                .IsRowVersion();

            entity.Property(o => o.Reference).IsRequired();

            entity.OwnsMany(o => o.Products, p =>
            {
                p.OwnsOne(r => r.Returnable, rt =>
                {
                    rt.Property(e => e.Vat).HasColumnType("decimal(10,2)");
                    rt.Property(e => e.WholeSalePrice).HasColumnType("decimal(10,2)");
                });

                p.Property(e => e.Vat).HasColumnType("decimal(10,2)");
                p.Property(e => e.WholeSalePrice).HasColumnType("decimal(10,2)");
                p.Property(e => e.WholeSalePricePerUnit).HasColumnType("decimal(10,2)");

                p.ToTable("PurchaseOrderProducts");
            });

            entity.OwnsOne(c => c.Vendor, c =>
            {
                c.Property(e => e.Name).IsRequired();
                c.Property(e => e.Email).IsRequired();
            });
            entity.OwnsOne(c => c.Sender, c =>
            {
                c.Property(e => e.Name).IsRequired();
                c.Property(e => e.Email).IsRequired();
            });

            entity.OwnsOne(c => c.ExpectedDelivery, cb =>
            {
                cb.OwnsOne(e => e.Address);
                cb.Property(e => e.DeliveryFeesWholeSalePrice).HasColumnType("decimal(10,2)");
            });            

            entity.HasKey(c => c.Id);
            entity.HasIndex(c => new { VendorId = c.Vendor.CompanyId, c.Reference}).IsUnique();
            entity.ToTable("PurchaseOrders");
        }
    }
}