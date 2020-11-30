using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class BusinessLegalConfiguration : IEntityTypeConfiguration<BusinessLegal>
    {
        public void Configure(EntityTypeBuilder<BusinessLegal> entity)
        {
            entity.Property(c => c.Siret).IsRequired();
            entity.Property(c => c.VatIdentifier);

            entity.OwnsOne(c => c.Address, e => {
                e.ToTable("BusinessLegalAddresses");
            });

            entity.OwnsOne(c => c.Declaration, d =>
            {
                d.OwnsMany(c => c.Ubos, e =>
                {
                    e.OwnsOne(c => c.Address);
                    e.OwnsOne(c => c.BirthPlace);

                    e.HasIndex(c => c.Identifier);
                    e.HasIndex(c => c.Id).IsUnique();

                    e.ToTable("DeclarationUbos");
                });

                d.ToTable("Declarations");
            });
        }
    }
}
