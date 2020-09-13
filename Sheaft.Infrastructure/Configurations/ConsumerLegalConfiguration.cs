using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Interop.Enums;

namespace Sheaft.Infrastructure
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
