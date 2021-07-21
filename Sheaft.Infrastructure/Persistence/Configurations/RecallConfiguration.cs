using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class RecallConfiguration : IEntityTypeConfiguration<Recall>
    {
        public void Configure(EntityTypeBuilder<Recall> entity)
        {
            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn);
            entity.Ignore(c => c.DomainEvents);

            entity.HasOne(c => c.Producer).WithMany().HasForeignKey(c => c.ProducerId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(c => c.Batches).WithOne().HasForeignKey(c => c.RecallId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(c => c.Products).WithOne().HasForeignKey(c => c.RecallId).OnDelete(DeleteBehavior.Cascade);

            entity.HasKey(c => c.Id);
            entity.ToTable("Recalls");
        }
    }
}