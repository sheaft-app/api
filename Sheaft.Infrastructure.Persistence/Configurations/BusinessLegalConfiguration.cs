using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class BusinessLegalConfiguration : IEntityTypeConfiguration<BusinessLegal>
    {
        public void Configure(EntityTypeBuilder<BusinessLegal> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("BusinessUid");
            entity.Property<long>("UboDeclarationUid");

            entity.Property(c => c.Siret).IsRequired();
            entity.Property(c => c.VatIdentifier).IsRequired();

            entity.OwnsOne(c => c.Address, e => {
                e.ToTable("BusinessLegalAddresses");
            });

            entity.HasOne(c => c.UboDeclaration).WithOne().HasForeignKey<BusinessLegal>("UboDeclarationUid").OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(c => c.Business).WithOne().HasForeignKey<BusinessLegal>("BusinessUid").OnDelete(DeleteBehavior.NoAction);

            entity.HasIndex("BusinessUid");
            entity.HasIndex("UboDeclarationUid");
        }
    }
}
