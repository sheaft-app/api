using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class SettingConfiguration : IEntityTypeConfiguration<Setting>
    {
        public void Configure(EntityTypeBuilder<Setting> entity)
        {
            entity.Property(c => c.Name).IsRequired();
            
            entity.HasKey(c=>c.Id);
            entity.HasIndex(c => c.Kind).IsUnique();
            entity.ToTable("Settings");
        }
    }
}