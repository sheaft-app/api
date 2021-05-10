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
            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn);
            entity.Property(c => c.RowVersion).IsRowVersion();

            if (!_isAdmin)
                entity.HasQueryFilter(p => !p.RemovedOn.HasValue);

            entity.Property(c => c.Name).IsRequired();

            entity.HasOne(c => c.Department).WithMany().HasForeignKey(c => c.DepartmentId)
                .OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(c => c.Winner).WithMany().HasForeignKey(c => c.WinnerId).OnDelete(DeleteBehavior.NoAction);

            entity.HasKey(c => c.Id);
            entity.ToTable("Rewards");
        }
    }
}