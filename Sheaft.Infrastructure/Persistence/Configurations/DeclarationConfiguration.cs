using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class DeclarationConfiguration : IEntityTypeConfiguration<Declaration>
    {
        public void Configure(EntityTypeBuilder<Declaration> entity)
        {
            entity.HasMany(c => c.Ubos).WithOne().HasForeignKey(c => c.DeclarationId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Ignore(c => c.DomainEvents);
            
            entity.HasKey(c => c.Id);

            entity.HasIndex(c => c.Identifier);
            entity.ToTable("Declarations");
        }
    }
}