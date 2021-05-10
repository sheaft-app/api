using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class UboConfiguration : IEntityTypeConfiguration<Ubo>
    {
        public void Configure(EntityTypeBuilder<Ubo> entity)
        {
            entity.HasKey(c => c.Id);

            entity.OwnsOne(c => c.Address);
            entity.OwnsOne(c => c.BirthPlace);
            
            entity.HasIndex(c => c.Identifier);
            entity.ToTable("DeclarationUbos");
        }
    }
}