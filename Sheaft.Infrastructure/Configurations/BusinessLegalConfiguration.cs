using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure
{
    public class BusinessLegalConfiguration : IEntityTypeConfiguration<BusinessLegal>
    {
        public void Configure(EntityTypeBuilder<BusinessLegal> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("BusinessUid");

            entity.OwnsOne(c => c.Address, e => {
                e.ToTable("BusinessLegalAddresses");
            });

            entity.OwnsMany(c => c.Ubos, e => {
                e.OwnsOne(a => a.Address);
                e.OwnsOne(a => a.BirthAddress);

                e.ToTable("BusinessUbos");
            });

            entity.HasOne(c => c.Business).WithOne().HasForeignKey<BusinessLegal>("BusinessUid").OnDelete(DeleteBehavior.NoAction);

            entity.HasIndex("BusinessUid");
        }
    }
}
