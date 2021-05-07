using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> entity)
        {
            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn);
            entity.Property(c => c.Content).UseCollation("Latin1_general_CI_AI");
            
            entity.HasOne(c => c.User).WithMany().HasForeignKey(c =>c.UserId).OnDelete(DeleteBehavior.NoAction).IsRequired();

            entity.HasKey(c =>c.Id);
            entity.ToTable("Notifications");
        }
    }
}
