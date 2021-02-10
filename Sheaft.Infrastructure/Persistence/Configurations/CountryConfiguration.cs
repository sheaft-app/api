using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> entity)
        {
            entity.Property<long>("Uid");

            entity.Property(o => o.Name).IsRequired();
            entity.Property(o => o.Alpha2).IsRequired();

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Alpha2).IsUnique();
            entity.HasIndex("Uid", "Id", "Alpha2");

            entity.ToTable("Countries");
        }
    }
}
