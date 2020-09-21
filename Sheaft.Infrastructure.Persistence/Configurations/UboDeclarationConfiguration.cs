using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class UboDeclarationConfiguration : IEntityTypeConfiguration<UboDeclaration>
    {
        public void Configure(EntityTypeBuilder<UboDeclaration> entity)
        {
            entity.Property<long>("Uid");

            entity.HasMany(c => c.Ubos).WithOne().HasForeignKey("UboDeclarationUid").OnDelete(DeleteBehavior.Cascade);

            entity.HasKey("Uid");
            entity.HasIndex(c => c.Identifier);
            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex("Uid", "Id", "Identifier", "RemovedOn");
        }
    }
}
