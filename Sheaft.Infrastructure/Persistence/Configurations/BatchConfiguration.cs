using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class BatchConfiguration : IEntityTypeConfiguration<Batch>
    {
        private readonly bool _isAdmin;

        public BatchConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }
        
        public void Configure(EntityTypeBuilder<Batch> entity)
        {
            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn);
            entity.Property(c => c.RowVersion).IsRowVersion();
            entity.Property(c => c.RemovedOn);
            entity.Ignore(c => c.Fields);
            entity.Ignore(c => c.DomainEvents);
            
            if(!_isAdmin)
                entity.HasQueryFilter(p => !p.RemovedOn.HasValue);

            entity.Property(o => o.Number).UseCollation("Latin1_general_CI_AI").IsRequired();
            
            entity.HasOne(c => c.Producer).WithMany().HasForeignKey(c => c.ProducerId).OnDelete(DeleteBehavior.NoAction).IsRequired();
            entity.HasOne(c => c.Definition).WithMany().HasForeignKey(c => c.DefinitionId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(c => c.Observations).WithOne().HasForeignKey(c => c.BatchId).OnDelete(DeleteBehavior.Cascade);
            
            entity.HasKey(c => c.Id);
            entity.HasIndex(c => new {c.ProducerId, c.Number}).IsUnique();
            entity.ToTable("Batches");
        }
    }
}