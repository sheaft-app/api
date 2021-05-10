using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class QuickOrderConfiguration : IEntityTypeConfiguration<QuickOrder>
    {
        private readonly bool _isAdmin;

        public QuickOrderConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }

        public void Configure(EntityTypeBuilder<QuickOrder> entity)
        {
            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn);
            entity.Property(c => c.RowVersion).IsRowVersion();

            if (!_isAdmin)
                entity.HasQueryFilter(p => !p.RemovedOn.HasValue);

            entity.Property(c => c.Name).UseCollation("Latin1_general_CI_AI").IsRequired();

            entity.HasMany(o => o.Products).WithOne().HasForeignKey(c => c.QuickOrderId)
                .OnDelete(DeleteBehavior.NoAction).IsRequired();
            entity.HasOne(c => c.User).WithMany().HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.Ignore(c => c.DomainEvents);

            entity.HasKey(c => c.Id);
            entity.ToTable("QuickOrders");
        }
    }
}