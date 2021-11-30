using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Database.Configurations
{
    public class QuickOrderConfiguration : IEntityTypeConfiguration<QuickOrder>
    {
        private readonly bool _isAdmin;

        public QuickOrderConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }

        public void Configure(EntityTypeBuilder<QuickOrder> entity)
        {
            if (!_isAdmin)
                entity.HasQueryFilter(p => !p.Removed);

            entity.Ignore(c => c.DomainEvents);

            entity
                .Property(c => c.UpdatedOn)
                .IsRowVersion();

            entity
                .Property(c => c.Name)
                .UseCollation("Latin1_general_CI_AI")
                .IsRequired();

            entity.OwnsMany(o => o.Products, p =>
            {
                p.HasOne<Catalog>()
                    .WithMany()
                    .HasForeignKey(e => e.CatalogId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                p.HasOne<Product>()
                    .WithMany()
                    .HasForeignKey(e => e.CatalogId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                p.ToTable("QuickOrderProducts");
            });

            entity
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasKey(c => c.Id);
            entity.ToTable("QuickOrders");
        }
    }
}