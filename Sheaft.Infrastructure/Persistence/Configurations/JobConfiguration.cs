using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("UserUid");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();

            entity.Property(c => c.Name).IsRequired();
            entity.Property(c => c.Status).IsRequired();
            entity.Property(c => c.Kind).IsRequired();

            entity.HasOne(o => o.User).WithMany().HasForeignKey("UserUid").OnDelete(DeleteBehavior.Cascade);

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex("UserUid");
            entity.HasIndex("Uid", "Id", "UserUid", "RemovedOn");

            entity.ToTable("Jobs");
        }
    }
}
