using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class BusinessConfiguration : IEntityTypeConfiguration<Business>
    {
        public void Configure(EntityTypeBuilder<Business> entity)
        {
            var closings = entity.Metadata.FindNavigation(nameof(Business.Closings));
            closings.SetPropertyAccessMode(PropertyAccessMode.Field);
            
            entity.HasMany(c => c.Closings).WithOne().HasForeignKey("BusinessUid").OnDelete(DeleteBehavior.Cascade);
        }
    }
}
