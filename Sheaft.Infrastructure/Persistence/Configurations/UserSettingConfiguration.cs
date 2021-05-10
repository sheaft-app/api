using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class UserSettingConfiguration : IEntityTypeConfiguration<UserSetting>
    {
        public void Configure(EntityTypeBuilder<UserSetting> entity)
        {
            entity.Property(c => c.Value).IsRequired();
            
            entity.HasKey(c=> new {c.UserId, c.SettingId});

            entity.HasOne(c => c.Setting).WithMany().HasForeignKey(c=>c.SettingId).OnDelete(DeleteBehavior.Cascade).IsRequired();
            
            entity.ToTable("UserSettings");
        }
    }
}