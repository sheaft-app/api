using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class BatchDefinitionConfiguration : IEntityTypeConfiguration<BatchDefinition>
    {
        private readonly bool _isAdmin;

        public BatchDefinitionConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }
        
        public void Configure(EntityTypeBuilder<BatchDefinition> entity)
        {
            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn);
            entity.Property(c => c.RowVersion).IsRowVersion();
            entity.Property(c => c.RemovedOn);
            entity.Ignore(c => c.FieldDefinitions);
            
            if(!_isAdmin)
                entity.HasQueryFilter(p => !p.RemovedOn.HasValue);

            entity.Property(o => o.Name).UseCollation("Latin1_general_CI_AI").IsRequired();
            
            entity.HasOne(c => c.Producer).WithMany().HasForeignKey(c => c.ProducerId).OnDelete(DeleteBehavior.Cascade).IsRequired();
            
            entity.HasKey(c => c.Id);
            entity.ToTable("BatchDefinitions");
        }
    }
}