using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class UserPointConfiguration : IEntityTypeConfiguration<UserPoint>
    {
        public void Configure(EntityTypeBuilder<UserPoint> entity)
        {
            entity.HasKey(c =>c.Id);
            entity.ToTable("UserPoints");
        }
    }
}