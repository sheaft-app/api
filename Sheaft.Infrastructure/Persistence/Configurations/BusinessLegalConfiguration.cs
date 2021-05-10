using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class BusinessLegalConfiguration : IEntityTypeConfiguration<BusinessLegal>
    {
        public void Configure(EntityTypeBuilder<BusinessLegal> entity)
        {
            entity.Property(c => c.Siret).IsRequired();
            entity.Property(c => c.VatIdentifier);

            entity.OwnsOne(c => c.Address);
            entity.HasOne(c => c.Declaration).WithOne().HasForeignKey<BusinessLegal>(c => c.DeclarationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
