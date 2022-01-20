using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Database.Configurations
{
    public class RecallConfiguration : IEntityTypeConfiguration<Recall>
    {
        private readonly bool _isAdmin;

        public RecallConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }

        public void Configure(EntityTypeBuilder<Recall> entity)
        {
            if (!_isAdmin)
                entity.HasQueryFilter(p => !p.Removed);

            entity.Ignore(c => c.DomainEvents);

            entity
                .Property(c => c.UpdatedOn)
                .IsRowVersion();

            entity
                .HasOne(c => c.Supplier)
                .WithMany()
                .HasForeignKey(c => c.SupplierId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity
                .OwnsMany(c => c.Batches, p =>
                {
                    p.HasOne<BatchNumber>()
                        .WithMany()
                        .HasForeignKey(bn => bn.BatchNumberId)
                        .OnDelete(DeleteBehavior.Restrict);
                    
                    p.ToTable("RecallBatchNumbers");
                });

            entity
                .OwnsMany(c => c.Products, p =>
                {
                    p.ToTable("RecallProducts");
                });

            entity
                .HasMany(c => c.Clients)
                .WithOne()
                .HasForeignKey(c => c.RecallId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasKey(c => c.Id);
            entity.ToTable("Recalls");
        }
    }

    public class RecallClientConfiguration : IEntityTypeConfiguration<RecallClient>
    {
        public void Configure(EntityTypeBuilder<RecallClient> entity)
        {
            entity.HasOne<User>().WithMany().HasForeignKey(c => c.ClientId).OnDelete(DeleteBehavior.Restrict);

            entity.HasKey(c => new { c.RecallId, c.ClientId });
            entity.ToTable("RecallClients");
        }
    }
}