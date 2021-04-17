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
            entity.Property<long>("Uid");
            entity.Property<long>("OrderUid");
            entity.Property<long>("CardUid");
            entity.Property<long?>("PreAuthorizedPayinUid");
            
            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();
            
            if(!_isAdmin)
                entity.HasQueryFilter(p => !p.RemovedOn.HasValue);
            
            entity.Property(o => o.Debited).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Remaining).HasColumnType("decimal(10,2)");

            entity.HasOne(c => c.Order).WithMany().HasForeignKey("OrderUid").OnDelete(DeleteBehavior.NoAction).IsRequired();
            entity.HasOne(c => c.Card).WithMany().HasForeignKey("CardUid").OnDelete(DeleteBehavior.Cascade).IsRequired();
            entity.HasOne(c => c.PreAuthorizedPayin).WithOne().HasForeignKey<PreAuthorization>("PreAuthorizedPayinUid").OnDelete(DeleteBehavior.NoAction);

            entity.HasKey("Uid");

            entity.Ignore(c => c.DomainEvents);
            
            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex(c => c.Identifier);
            entity.HasIndex("OrderUid");
            entity.HasIndex("CardUid");
            entity.HasIndex("PreAuthorizedPayinUid");
            entity.HasIndex("Uid", "Id", "OrderUid", "CardUid", "RemovedOn");

            entity.ToTable("PreAuthorizations");
        }
    }
}