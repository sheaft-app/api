﻿using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure
{
    public class NationalityConfiguration : IEntityTypeConfiguration<Nationality>
    {
        public void Configure(EntityTypeBuilder<Nationality> entity)
        {
            entity.Property<long>("Uid");

            entity.Property(o => o.Name).IsRequired();
            entity.Property(o => o.Alpha2).IsRequired();

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Alpha2).IsUnique();
            entity.HasIndex("Uid", "Id", "Alpha2");

            entity.ToTable("Nationalities");
        }
    }
}