using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class ProfileInformationConfiguration : IEntityTypeConfiguration<ProfileInformation>
    {
        public void Configure(EntityTypeBuilder<ProfileInformation> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("UserUid");

            entity.HasKey("Uid");

            entity.HasOne<User>().WithOne(u => u.ProfileInformation).HasForeignKey<ProfileInformation>("UserUid")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(c => c.Pictures).WithOne().HasForeignKey("ProfileInformationUid").OnDelete(DeleteBehavior.Cascade);

            var pictures = entity.Metadata.FindNavigation(nameof(ProfileInformation.Pictures));
            pictures.SetPropertyAccessMode(PropertyAccessMode.Field);
            
            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex("UserUid");
            entity.HasIndex("Uid", "UserUid");

            entity.ToTable("ProfileInformations");
        }
    }
}