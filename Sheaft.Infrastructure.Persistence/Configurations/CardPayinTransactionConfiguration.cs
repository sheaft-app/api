using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class CardPayinTransactionConfiguration : IEntityTypeConfiguration<CardPayinTransaction>
    {
        public void Configure(EntityTypeBuilder<CardPayinTransaction> entity)
        {
            entity.Property<long>("CardUid");

            entity.HasOne(c => c.Card).WithMany().HasForeignKey("CardUid").OnDelete(DeleteBehavior.NoAction);

            entity.HasIndex("CardUid");
        }
    }
}
