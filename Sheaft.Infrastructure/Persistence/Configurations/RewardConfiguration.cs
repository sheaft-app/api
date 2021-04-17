using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class RewardConfiguration : IEntityTypeConfiguration<Reward>
    {
        private readonly bool _isAdmin;

        public RewardConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }

        public void Configure(EntityTypeBuilder<Reward> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("LevelUid");
            entity.Property<long>("DepartmentUid");
            entity.Property<long?>("WinnerUid");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();
            
            if(!_isAdmin)
                entity.HasQueryFilter(p => !p.RemovedOn.HasValue);

            entity.Property(c => c.Name).IsRequired();

            entity.HasOne(c => c.Department).WithMany().HasForeignKey("DepartmentUid").OnDelete(DeleteBehavior.NoAction).IsRequired();
            entity.HasOne(c => c.Winner).WithMany().HasForeignKey("WinnerUid").OnDelete(DeleteBehavior.NoAction);

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex("LevelUid");
            entity.HasIndex("DepartmentUid");
            entity.HasIndex("Uid", "Id", "DepartmentUid", "LevelUid");

            entity.ToTable("Rewards");
        }
    }
}
