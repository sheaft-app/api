using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class PreAuthorizationConfiguration : IEntityTypeConfiguration<PreAuthorization>
    {
        public void Configure(EntityTypeBuilder<PreAuthorization> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("OrderUid");
            entity.Property<long>("CardUid");

            entity.Property(o => o.Debited).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Remaining).HasColumnType("decimal(10,2)");

            entity.HasOne(c => c.Order).WithMany().HasForeignKey("OrderUid").OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(c => c.Card).WithMany().HasForeignKey("CardUid").OnDelete(DeleteBehavior.Cascade);
            
            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex(c => c.Identifier);
            entity.HasIndex("OrderUid");
            entity.HasIndex("CardUid");
            entity.HasIndex("Uid", "Id", "OrderUid", "CardUid", "RemovedOn");

            entity.ToTable("PreAuthorizations");
        }
    }
}