using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Infrastructure.Persistence.Database.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        private readonly bool _isAdmin;

        public ProductConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }

        public void Configure(EntityTypeBuilder<Product> entity)
        {
            if (!_isAdmin)
                entity.HasQueryFilter(p => !p.Removed);

            entity.Ignore(c => c.DomainEvents);

            entity
                .Property(c => c.UpdatedOn)
                .IsRowVersion();

            entity
                .Property(o => o.Name)
                .UseCollation("Latin1_general_CI_AI")
                .IsRequired();

            entity
                .Property(o => o.Reference)
                .IsRequired();

            entity
                .Property(o => o.QuantityPerUnit)
                .HasColumnType("decimal(10,3)");

            entity
                .Property(o => o.Vat)
                .HasColumnType("decimal(10,2)");

            entity
                .Property(o => o.Weight)
                .HasColumnType("decimal(10,2)");

            entity
                .Property(o => o.VisibleTo)
                .HasDefaultValue(VisibleToKind.None);

            entity.HasOne(c => c.Returnable)
                .WithMany()
                .HasForeignKey(c => c.ReturnableId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne<Company>()
                .WithMany()
                .HasForeignKey(c => c.SupplierId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.OwnsMany(c => c.Tags, t =>
            {
                t.ToTable("ProductTags");
            });

            entity.OwnsMany(c => c.Pictures, p =>
            {
                p.ToTable("ProductPictures");
            });

            entity.HasKey(c => c.Id);
            entity.HasIndex(c => new { c.SupplierId, c.Reference }).IsUnique();

            entity.ToTable("Products");
        }
    }
}