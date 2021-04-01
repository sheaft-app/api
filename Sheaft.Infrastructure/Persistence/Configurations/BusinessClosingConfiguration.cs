using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class BusinessClosingConfiguration : IEntityTypeConfiguration<BusinessClosing>
    {
        public void Configure(EntityTypeBuilder<BusinessClosing> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("BusinessUid");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();
            
            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex("BusinessUid");
            entity.HasIndex("Uid", "Id", "BusinessUid");

            entity.ToTable("BusinessClosings");
        }
    }
}