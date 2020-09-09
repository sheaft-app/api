using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Interop.Enums;

namespace Sheaft.Infrastructure
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
            entity.Property(c => c.Identifier).IsRequired();
            entity.Property(c => c.Owner).IsRequired();

            entity.HasDiscriminator(c => c.Kind)
                .HasValue<TransferPayment>(PaymentKind.Transfer)
                .HasValue<CardPayment>(PaymentKind.Card);

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex(c => c.Identifier).IsUnique();
            entity.HasIndex("UserUid");
            entity.HasIndex("Uid", "Id", "Identifier", "UserUid", "CreatedOn");

            entity.ToTable("PaymentMethods");
        }
    }
}
