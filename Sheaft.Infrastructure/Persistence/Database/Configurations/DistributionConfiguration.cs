using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Database.Configurations
{
    public class DistributionConfiguration : IEntityTypeConfiguration<Distribution>
    {
        private readonly bool _isAdmin;

        public DistributionConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }

        public void Configure(EntityTypeBuilder<Distribution> entity)
        {
            if (!_isAdmin)
                entity.HasQueryFilter(p => !p.Removed);

            entity.Ignore(c => c.DomainEvents);

            entity
                .Property(c => c.UpdatedOn)
                .IsRowVersion();

            entity
                .Property(c => c.Name)
                .UseCollation("Latin1_general_CI_AI");

            entity.OwnsOne(c => c.Address);

            entity.OwnsMany(c => c.OpeningHours, oh =>
            {
                oh.ToTable("DistributionOpeningHours");
            });

            entity.OwnsMany(c => c.Closings, oh =>
            {
                oh.ToTable("DistributionClosings");
            });

            entity.HasOne(c => c.Supplier)
                .WithMany()
                .HasForeignKey(c => c.SupplierId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasKey(c => c.Id);
            entity.ToTable("Distributions");
        }
    }
}