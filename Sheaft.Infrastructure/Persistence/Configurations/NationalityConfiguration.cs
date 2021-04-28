using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class NationalityConfiguration : IEntityTypeConfiguration<Nationality>
    {
        public void Configure(EntityTypeBuilder<Nationality> entity)
        {
            entity.Property(o => o.Name).IsRequired();
            entity.Property(o => o.Alpha2).IsRequired();

            entity.HasKey(c =>c.Id);

            entity.HasIndex(c => c.Alpha2).IsUnique();
            entity.ToTable("Nationalities");
        }
    }
}
