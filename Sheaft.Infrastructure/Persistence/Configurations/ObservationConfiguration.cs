using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class ObservationConfiguration : IEntityTypeConfiguration<Observation>
    {
        public void Configure(EntityTypeBuilder<Observation> entity)
        {
            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn);
            entity.Ignore(c => c.DomainEvents);

            entity.HasOne(c => c.User).WithMany().HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(c => c.Producer).WithMany().HasForeignKey(c => c.ProducerId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(c => c.Replies).WithOne().HasForeignKey(c => c.ReplyToId).OnDelete(DeleteBehavior.NoAction);
            entity.HasMany(c => c.Batches).WithOne().HasForeignKey(c => c.ObservationId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(c => c.Products).WithOne().HasForeignKey(c => c.ObservationId).OnDelete(DeleteBehavior.Cascade);

            entity.HasKey(c => c.Id);
            entity.ToTable("Observations");
        }
    }
}