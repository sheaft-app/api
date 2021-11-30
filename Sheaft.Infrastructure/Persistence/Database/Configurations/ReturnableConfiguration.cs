using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Database.Configurations
{
    public class ReturnableConfiguration : IEntityTypeConfiguration<Returnable>
    {
        private readonly bool _isAdmin;

        public ReturnableConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }

        public void Configure(EntityTypeBuilder<Returnable> entity)
        {
            if (!_isAdmin)
                entity.HasQueryFilter(p => !p.Removed);

            entity
                .Property(c => c.UpdatedOn)
                .IsRowVersion();

            entity
                .Property(o => o.Name)
                .UseCollation("Latin1_general_CI_AI")
                .IsRequired();

            entity
                .Property(o => o.WholeSalePrice)
                .HasColumnType("decimal(10,2)");

            entity
                .Property(o => o.Vat)
                .HasColumnType("decimal(10,2)");

            entity.Ignore(o => o.VatPrice);
            entity.Ignore(o => o.OnSalePrice);
            
            entity.HasKey(c => c.Id);
            entity.ToTable("Returnables");
        }
    }
}