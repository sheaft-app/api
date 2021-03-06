﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class LevelConfiguration : IEntityTypeConfiguration<Level>
    {
        private readonly bool _isAdmin;

        public LevelConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }

        public void Configure(EntityTypeBuilder<Level> entity)
        {
            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn);
            
            if(!_isAdmin)
                entity.HasQueryFilter(p => !p.RemovedOn.HasValue);

            entity.Property(o => o.Name).IsRequired();

            entity.HasMany(c => c.Rewards).WithOne(c => c.Level).HasForeignKey(c =>c.LevelId).OnDelete(DeleteBehavior.Cascade).IsRequired();

            entity.HasKey(c =>c.Id);
            entity.ToTable("Levels");
        }
    }
}
