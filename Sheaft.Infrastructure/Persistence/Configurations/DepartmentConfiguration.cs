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
            entity.Property<long>("Uid");
            entity.Property<long>("RegionUid");
            entity.Property<long>("LevelUid");

            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();

            entity.Property(c => c.Name).IsRequired();
            entity.Property(c => c.Code).IsRequired();
            entity.Property(c => c.ProducersCount).HasDefaultValue(0);
            entity.Property(c => c.StoresCount).HasDefaultValue(0);
            entity.Property(c => c.ConsumersCount).HasDefaultValue(0);
            entity.Property(c => c.Points).HasDefaultValue(0);
            entity.Property(c => c.Position).HasDefaultValue(0);

            entity.HasOne(c => c.Region).WithMany().HasForeignKey("RegionUid").OnDelete(DeleteBehavior.Cascade).IsRequired();
            entity.HasOne(c => c.Level).WithMany().HasForeignKey("LevelUid").OnDelete(DeleteBehavior.NoAction).IsRequired();

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Code).IsUnique();
            entity.HasIndex(c => c.Id).IsUnique();

            entity.HasIndex("RegionUid");
            entity.HasIndex("LevelUid");
            entity.HasIndex("Uid", "Id", "RegionUid", "LevelUid");

            entity.ToTable("Departments");
        }
    }
}
