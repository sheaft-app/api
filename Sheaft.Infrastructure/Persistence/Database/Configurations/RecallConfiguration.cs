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
                .HasMany(c => c.Batches)
                .WithOne()
                .HasForeignKey(c => c.RecallId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity
                .HasMany(c => c.Products)
                .WithOne()
                .HasForeignKey(c => c.RecallId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

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

    public class RecallProductConfiguration : IEntityTypeConfiguration<RecallProduct>
    {
        public void Configure(EntityTypeBuilder<RecallProduct> entity)
        {
            entity.HasOne<Product>().WithMany().HasForeignKey(c => c.ProductId).OnDelete(DeleteBehavior.Restrict);

            entity.HasKey(c => c.Id);
            entity.ToTable("RecallProducts");
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

    public class RecallBatchNumberConfiguration : IEntityTypeConfiguration<RecallBatchNumber>
    {
        public void Configure(EntityTypeBuilder<RecallBatchNumber> entity)
        {
            entity.HasOne<BatchNumber>().WithMany().HasForeignKey(c => c.BatchNumberId).OnDelete(DeleteBehavior.Restrict);

            entity.HasKey(c => c.Id);
            entity.ToTable("RecallBatchNumbers");
        }
    }
}