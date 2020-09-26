using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class UboConfiguration : IEntityTypeConfiguration<Ubo>
    {
        public void Configure(EntityTypeBuilder<Ubo> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("UboDeclarationUid");

            entity.OwnsOne(c => c.Address);
            entity.OwnsOne(c => c.BirthPlace);

            entity.HasKey("Uid");
            entity.HasIndex("UboDeclarationUid");
            entity.HasIndex(c => c.Identifier);
            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex("Uid", "UboDeclarationUid", "Id", "RemovedOn");
        }
    }
}
