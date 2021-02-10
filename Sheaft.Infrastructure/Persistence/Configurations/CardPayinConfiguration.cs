using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class CardPayinConfiguration : IEntityTypeConfiguration<CardPayin>
    {
        public void Configure(EntityTypeBuilder<CardPayin> entity)
        {
            entity.Property<long>("CardUid");
            entity.HasOne(c => c.Card).WithMany().HasForeignKey("CardUid").OnDelete(DeleteBehavior.NoAction);
            entity.HasIndex("CardUid");
        }
    }
}
