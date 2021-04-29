using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> entity)
        {
            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();
            
            entity.Ignore(c => c.DomainEvents);

            entity.HasMany(c => c.Pages).WithOne().HasForeignKey(c =>c.DocumentId).OnDelete(DeleteBehavior.Cascade).IsRequired();

            entity.HasKey(c =>c.Id);
            entity.HasIndex(c =>c.Identifier);
            entity.ToTable("Documents");
        }
    }
}