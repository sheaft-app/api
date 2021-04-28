using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class PreAuthorizationConfiguration : IEntityTypeConfiguration<PreAuthorization>
    {
        private readonly bool _isAdmin;

        public PreAuthorizationConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }

        public void Configure(EntityTypeBuilder<PreAuthorization> entity)
        {
            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();
            
            if(!_isAdmin)
                entity.HasQueryFilter(p => !p.RemovedOn.HasValue);
            
            entity.Property(o => o.Debited).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Remaining).HasColumnType("decimal(10,2)");

            entity.HasOne(c => c.Order).WithMany().HasForeignKey(c=>c.OrderId).OnDelete(DeleteBehavior.NoAction).IsRequired();
            entity.HasOne(c => c.Card).WithMany().HasForeignKey(c=>c.CardId).OnDelete(DeleteBehavior.Cascade).IsRequired();
            entity.HasOne(c => c.PreAuthorizedPayin).WithOne().HasForeignKey<PreAuthorization>(c=>c.PreAuthorizedPayinId).OnDelete(DeleteBehavior.NoAction);

            entity.HasKey(c=>c.Id);

            entity.Ignore(c => c.DomainEvents);
            entity.HasIndex(c => c.Identifier);
            
            entity.ToTable("PreAuthorizations");
        }
    }
}