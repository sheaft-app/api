using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("UserUid");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();

            entity.Property(c => c.Name).IsRequired();

            entity.HasDiscriminator(c => c.Kind)
                .HasValue<BankAccount>(PaymentKind.BankAccount)
                .HasValue<Card>(PaymentKind.Card);

            entity.HasOne(c => c.User).WithMany().HasForeignKey("UserUid").OnDelete(DeleteBehavior.Cascade);

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex(c => c.Identifier);
            entity.HasIndex("UserUid");
            entity.HasIndex("Uid", "Id", "Identifier", "UserUid", "RemovedOn");

            entity.ToTable("PaymentMethods");
        }
    }
}
