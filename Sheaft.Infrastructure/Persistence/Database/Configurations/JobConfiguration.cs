using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Database.Configurations
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
            if (!_isAdmin)
                entity.HasQueryFilter(p => !p.Removed);

            entity.Ignore(c => c.DomainEvents);
            
            entity
                .Property(c => c.UpdatedOn)
                .IsRowVersion();

            entity
                .Property(c => c.Name)
                .UseCollation("Latin1_general_CI_AI")
                .IsRequired();

            entity
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasKey(c => c.Id);
            entity.ToTable("Jobs");
        }
    }
}