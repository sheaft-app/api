using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Database.Configurations
{
    public class ObservationConfiguration : IEntityTypeConfiguration<Observation>
    {
        private readonly bool _isAdmin;

        public ObservationConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }
        
        public void Configure(EntityTypeBuilder<Observation> entity)
        {
            if (!_isAdmin)
                entity.HasQueryFilter(p => !p.Removed);

            entity.Ignore(c => c.DomainEvents);

            entity
                .Property(c => c.UpdatedOn)
                .IsRowVersion();
            
            entity
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            entity
                .HasOne(c => c.Supplier)
                .WithMany()
                .HasForeignKey(c => c.SupplierId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity
                .HasMany(c => c.Replies)
                .WithOne()
                .HasForeignKey(c => c.ReplyToId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.OwnsMany(c => c.BatchNumbers, b =>
            {
                b.HasOne<BatchNumber>().WithMany().HasForeignKey(bn => bn.BatchNumberId).OnDelete(DeleteBehavior.Restrict);
                b.ToTable("ObservationBatchNumbers");
            });
            
            entity.OwnsMany(c => c.Products, b =>
            {
                b.ToTable("ObservationProducts");
            });

            entity.HasKey(c => c.Id);
            entity.ToTable("Observations");
        }
    }
}