using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> entity)
        {
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();

            entity.Property(c => c.Name).UseCollation("Latin1_general_CI_AI").IsRequired();
            entity.Property(c => c.Code).IsRequired();
            entity.Property(c => c.ProducersCount).HasDefaultValue(0);
            entity.Property(c => c.StoresCount).HasDefaultValue(0);
            entity.Property(c => c.ConsumersCount).HasDefaultValue(0);
            entity.Property(c => c.Points).HasDefaultValue(0);
            entity.Property(c => c.Position).HasDefaultValue(0);

            entity.HasOne(c => c.Region).WithMany().HasForeignKey(c =>c.RegionId).OnDelete(DeleteBehavior.Cascade).IsRequired();
            entity.HasOne(c => c.Level).WithMany().HasForeignKey(c =>c.LevelId).OnDelete(DeleteBehavior.NoAction).IsRequired();

            entity.HasKey(c => c.Id);

            entity.HasIndex(c => c.Code).IsUnique();
            entity.ToTable("Departments");
        }
    }
}
