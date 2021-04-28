using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class RegionConfiguration : IEntityTypeConfiguration<Region>
    {
        public void Configure(EntityTypeBuilder<Region> entity)
        {
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();

            entity.Property(c => c.Name).IsRequired();
            entity.Property(c => c.Code).IsRequired();
            entity.Property(c => c.ProducersCount).HasDefaultValue(0);
            entity.Property(c => c.StoresCount).HasDefaultValue(0);
            entity.Property(c => c.ConsumersCount).HasDefaultValue(0);
            entity.Property(c => c.Points).HasDefaultValue(0);
            entity.Property(c => c.Position).HasDefaultValue(0);

            entity.HasKey(c=>c.Id);
            entity.HasIndex(c => c.Code).IsUnique();

            entity.ToTable("Regions");
        }
    }
}
