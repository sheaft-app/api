using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.AgreementManagement;
using Sheaft.Domain.CustomerManagement;
using Sheaft.Domain.ProductManagement;
using Sheaft.Domain.SupplierManagement;

namespace Sheaft.Infrastructure.Persistence.Configurations;

internal class AgreementConfiguration : IEntityTypeConfiguration<Agreement>
{
    public void Configure(EntityTypeBuilder<Agreement> builder)
    {
        builder.HasKey(c => c.Id);

        builder.OwnsMany(a => a.DeliveryDays, dd =>
        {
            dd.Property(c => c.Value)
                .HasColumnName("DayOfWeek");
            
            dd.WithOwner().HasForeignKey("AgreementId");
            dd.Property<long>("Id");
            dd.HasKey("Id");
            
            dd.ToTable("Agreement_DeliveryDays");
        });
        
        builder
            .Property(c => c.Id)
            .HasMaxLength(Constants.IDS_LENGTH);

        builder
            .HasOne<Customer>()
            .WithMany()
            .HasForeignKey(c => c.CustomerId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder
            .HasOne<Supplier>()
            .WithMany()
            .HasForeignKey(c => c.SupplierId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder
            .HasOne<Catalog>()
            .WithMany()
            .HasForeignKey(c => c.CatalogId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.ToTable("Agreement");
    }
}