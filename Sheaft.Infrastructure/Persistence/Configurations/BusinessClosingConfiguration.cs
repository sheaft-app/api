using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class BusinessClosingConfiguration : IEntityTypeConfiguration<BusinessClosing>
    {
        public void Configure(EntityTypeBuilder<BusinessClosing> entity)
        {
            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();
            
            entity.HasKey(c=> c.Id);
            entity.ToTable("BusinessClosings");
        }
    }
}