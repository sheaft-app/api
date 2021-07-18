using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class RecallClientConfiguration : IEntityTypeConfiguration<RecallClient>
    {
        public void Configure(EntityTypeBuilder<RecallClient> entity)
        {
            entity.Property(c => c.CreatedOn);

            entity.HasOne(c => c.Client).WithMany().HasForeignKey(c => c.ClientId).OnDelete(DeleteBehavior.NoAction);
            
            entity.HasKey(c => new {c.ClientId, c.RecallId});
            entity.ToTable("RecallClients");
        }
    }
}