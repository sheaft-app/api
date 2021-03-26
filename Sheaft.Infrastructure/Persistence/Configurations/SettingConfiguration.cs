using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class SettingConfiguration : IEntityTypeConfiguration<Setting>
    {
        public void Configure(EntityTypeBuilder<Setting> entity)
        {
            entity.Property<long>("Uid");

            entity.Property(c => c.Name).IsRequired();

            entity.HasKey("Uid");
            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex(c => c.Kind).IsUnique();
            entity.HasIndex("Uid", "Id");
            entity.ToTable("Settings");
        }
    }
}