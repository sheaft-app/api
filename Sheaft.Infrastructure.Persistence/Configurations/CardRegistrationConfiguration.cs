using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class CardRegistrationConfiguration : IEntityTypeConfiguration<CardRegistration>
    {
        public void Configure(EntityTypeBuilder<CardRegistration> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("UserUid");

            entity.HasOne(c => c.User).WithMany().HasForeignKey("UserUid").OnDelete(DeleteBehavior.Cascade);
            
            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex(c => c.Identifier);
            entity.HasIndex("UserUid");
            entity.HasIndex("Uid", "Id", "UserUid", "RemovedOn");

            entity.ToTable("CardRegistrations");
        }
    }
}
