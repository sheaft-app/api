using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class ProducerTagConfiguration : IEntityTypeConfiguration<ProducerTag>
    {
        public void Configure(EntityTypeBuilder<ProducerTag> entity)
        {
            entity.HasOne(c => c.Tag).WithMany().HasForeignKey(c=>c.TagId).OnDelete(DeleteBehavior.Cascade).IsRequired();

            entity.HasKey(c => new {c.ProducerId, c.TagId});

            entity.ToTable("ProducerTags");
        }
    }
}
