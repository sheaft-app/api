using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
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
            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();
            
            if(!_isAdmin)
                entity.HasQueryFilter(p => !p.RemovedOn.HasValue);

            entity.Property(c => c.Name).IsRequired();

            entity.HasKey(c=>c.Id);
            entity.ToTable("Tags");
        }
    }
}
