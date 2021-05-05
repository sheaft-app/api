using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> entity)
        {
            entity.Property(o => o.Name).UseCollation("Latin1_general_CI_AI").IsRequired();
            entity.Property(o => o.Alpha2).IsRequired();

            entity.HasKey(c => c.Id);
            entity.HasIndex(c => c.Alpha2).IsUnique();
            
            entity.ToTable("Countries");
        }
    }
}
