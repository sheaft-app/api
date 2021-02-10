using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class ProducerTagConfiguration : IEntityTypeConfiguration<ProducerTag>
    {
        public void Configure(EntityTypeBuilder<ProducerTag> entity)
        {
            entity.Property<long>("ProducerUid");
            entity.Property<long>("TagUid");

            entity.HasOne(c => c.Tag).WithMany().HasForeignKey("TagUid").OnDelete(DeleteBehavior.Cascade);

            entity.HasKey("ProducerUid", "TagUid");

            entity.ToTable("ProducerTags");
        }
    }
}
