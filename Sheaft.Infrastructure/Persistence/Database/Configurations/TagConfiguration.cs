using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Database.Configurations
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        private readonly bool _isAdmin;

        public TagConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }

        public void Configure(EntityTypeBuilder<Tag> entity)
        {
            if (!_isAdmin)
                entity.HasQueryFilter(p => !p.Removed);

            entity.Property(c => c.UpdatedOn).IsRowVersion();

            entity
                .Property(c => c.Name)
                .UseCollation("Latin1_general_CI_AI")
                .IsRequired();

            entity.HasKey(c => c.Id);
            entity.ToTable("Tags");
        }
    }
}