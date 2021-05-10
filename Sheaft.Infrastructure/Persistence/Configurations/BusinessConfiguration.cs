using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class BusinessConfiguration : IEntityTypeConfiguration<Domain.Business>
    {
        public void Configure(EntityTypeBuilder<Domain.Business> entity)
        {
            entity.HasMany(c => c.Closings).WithOne().HasForeignKey(c => c.BusinessId).OnDelete(DeleteBehavior.Cascade).IsRequired();
        }
    }
}
