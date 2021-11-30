using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Database.Configurations
{
    public class SettingConfiguration : IEntityTypeConfiguration<Setting>
    {
        public void Configure(EntityTypeBuilder<Setting> entity)
        {
            entity
                .Property(c => c.Name)
                .UseCollation("Latin1_general_CI_AI")
                .IsRequired();

            entity.HasKey(c => c.Id);
            entity.HasIndex(c => c.Kind).IsUnique();
            entity.ToTable("Settings");
        }
    }
}