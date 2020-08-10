using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure
{
    public class RewardConfiguration : IEntityTypeConfiguration<Reward>
    {
        public void Configure(EntityTypeBuilder<Reward> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("LevelUid");
            entity.Property<long>("DepartmentUid");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();

            entity.Property(c => c.Name).IsRequired();

            entity.HasOne(c => c.Department).WithMany().HasForeignKey("DepartmentUid").OnDelete(DeleteBehavior.NoAction);

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex("LevelUid");
            entity.HasIndex("DepartmentUid");
            entity.HasIndex("Uid", "Id", "DepartmentUid", "LevelUid", "CreatedOn");

            entity.ToTable("Rewards");
        }
    }
}
