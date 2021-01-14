using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class CardConfiguration : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> entity)
        {
            entity.Property<long>("CardRegistrationUid");            
            entity.HasOne(c => c.CardRegistration).WithOne().HasForeignKey<Card>("CardRegistrationUid").OnDelete(DeleteBehavior.NoAction);
            entity.HasIndex("CardRegistrationUid");
        }
    }
}
