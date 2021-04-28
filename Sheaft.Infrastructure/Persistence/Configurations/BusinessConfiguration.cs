using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class BusinessConfiguration : IEntityTypeConfiguration<Domain.Business>
    {
        public void Configure(EntityTypeBuilder<Domain.Business> entity)
        {
            var closings = entity.Metadata.FindNavigation(nameof(Domain.Business.Closings));
            closings.SetPropertyAccessMode(PropertyAccessMode.Field);
            
            entity.HasMany(c => c.Closings).WithOne().HasForeignKey(c => c.BusinessId).OnDelete(DeleteBehavior.Cascade).IsRequired();
        }
    }
}
