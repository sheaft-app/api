using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class ConsumerLegalConfiguration : IEntityTypeConfiguration<ConsumerLegal>
    {
        public void Configure(EntityTypeBuilder<ConsumerLegal> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("ConsumerUid");

            entity.HasOne(c => c.Consumer).WithOne().HasForeignKey<ConsumerLegal>("ConsumerUid").OnDelete(DeleteBehavior.NoAction);

            entity.HasIndex("ConsumerUid");
        }
    }
}
