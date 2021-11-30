using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Database.Configurations
{
    public class BatchNumberConfiguration : IEntityTypeConfiguration<BatchNumber>
    {
        private readonly bool _isAdmin;

        public BatchNumberConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }
        
        public void Configure(EntityTypeBuilder<BatchNumber> entity)
        {
            if (!_isAdmin)
                entity.HasQueryFilter(p => !p.Removed);

            entity
                .Property(c => c.UpdatedOn)
                .IsRowVersion();
            
            entity
                .Property(o => o.Number)
                .UseCollation("Latin1_general_CI_AI")
                .IsRequired();
            
            entity
                .HasOne(c => c.CreatedBy)
                .WithMany()
                .HasForeignKey(c => c.CreatedById)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
            
            entity.HasKey(c => c.Id);
            entity.HasIndex(c => new {c.SupplierId, c.Number}).IsUnique();
            entity.ToTable("Batches");
        }
    }
}