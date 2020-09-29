using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("LegalUid");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();

            entity.OwnsMany(c => c.Pages, cb =>
            {
                cb.Property(o => o.Filename).IsRequired();
                cb.HasIndex(c => c.Id).IsUnique();

                cb.ToTable("DocumentPages");
            });

            entity.HasOne(c => c.Legal).WithMany().HasForeignKey("LegalUid").OnDelete(DeleteBehavior.Cascade);

            var pages = entity.Metadata.FindNavigation(nameof(Document.Pages));
            pages.SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex(c => c.Identifier);
            entity.HasIndex("LegalUid");
            entity.HasIndex("Uid", "Id", "LegalUid", "RemovedOn");

            entity.ToTable("Documents");
        }
    }
}
