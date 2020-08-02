using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure
{
    public class CompanyTagConfiguration : IEntityTypeConfiguration<CompanyTag>
    {
        public void Configure(EntityTypeBuilder<CompanyTag> entity)
        {
            entity.Property<long>("CompanyUid");
            entity.Property<long>("TagUid");

            entity.HasOne(c => c.Tag).WithMany().HasForeignKey("TagUid").OnDelete(DeleteBehavior.Cascade);

            entity.HasKey("CompanyUid", "TagUid");

            entity.ToTable("CompanyTags");
        }
    }
}
