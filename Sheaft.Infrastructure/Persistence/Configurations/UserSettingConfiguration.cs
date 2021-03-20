using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class UserSettingConfiguration : IEntityTypeConfiguration<UserSetting>
    {
        public void Configure(EntityTypeBuilder<UserSetting> entity)
        {
            entity.Property<long>("UserUid");
            entity.Property<long>("SettingUid");
            entity.Property(c => c.Value).IsRequired();
            
            entity.HasKey("UserUid", "SettingUid");

            entity.HasOne(c => c.Setting).WithMany().HasForeignKey("SettingUid").OnDelete(DeleteBehavior.Cascade);
            
            entity.ToTable("UserSettings");
        }
    }
}