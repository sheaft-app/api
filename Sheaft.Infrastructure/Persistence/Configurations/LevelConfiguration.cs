﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class LevelConfiguration : IEntityTypeConfiguration<Level>
    {
        public void Configure(EntityTypeBuilder<Level> entity)
        {
            entity.Property<long>("Uid");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();

            entity.Property(o => o.Name).IsRequired();

            entity.HasMany(c => c.Rewards).WithOne(c => c.Level).HasForeignKey("LevelUid").OnDelete(DeleteBehavior.Cascade);

            var rewards = entity.Metadata.FindNavigation(nameof(Level.Rewards));
            rewards.SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex("Uid", "Id", "RemovedOn");

            entity.ToTable("Levels");
        }
    }
}