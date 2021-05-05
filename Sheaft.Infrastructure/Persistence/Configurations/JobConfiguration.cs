using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        private readonly bool _isAdmin;

        public JobConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }

        public void Configure(EntityTypeBuilder<Job> entity)
        {
            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();
            
            if(!_isAdmin)
                entity.HasQueryFilter(p => !p.RemovedOn.HasValue);

            entity.Property(c => c.Name).UseCollation("Latin1_general_CI_AI").IsRequired();
            entity.Property(c => c.Status).IsRequired();
            entity.Property(c => c.Kind).IsRequired();

            entity.HasOne(o => o.User).WithMany().HasForeignKey(c =>c.UserId).OnDelete(DeleteBehavior.Cascade).IsRequired();

            entity.Ignore(c => c.DomainEvents);
            
            entity.HasKey(c =>c.Id);
            entity.ToTable("Jobs");
        }
    }
}
