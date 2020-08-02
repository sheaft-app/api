using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure
{
    public class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("UserUid");

            entity.Property(c => c.CreatedOn).ValueGeneratedOnAdd();
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken().ValueGeneratedOnUpdate();
            entity.Property(c => c.Name).IsRequired();
            entity.Property(c => c.Status).IsRequired();
            entity.Property(c => c.Kind).IsRequired();

            entity.HasOne(o => o.User).WithMany().HasForeignKey("UserUid").OnDelete(DeleteBehavior.Cascade);

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex("UserUid");
            entity.HasIndex("Uid", "Id", "UserUid", "CreatedOn");

            entity.ToTable("Jobs");
        }
    }
}
