using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class PageConfiguration : IEntityTypeConfiguration<Page>
    {
        public void Configure(EntityTypeBuilder<Page> entity)
        {
            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn);
            entity.Property(c => c.RowVersion).IsRowVersion();
            entity.Property(c => c.Filename).IsRequired();

            entity.HasKey(c => c.Id);
            entity.ToTable("DocumentPages");
        }
    }
}