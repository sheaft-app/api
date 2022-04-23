﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.AgreementManagement;

namespace Sheaft.Infrastructure.Persistence.Configurations;

internal class AgreementConfiguration : IEntityTypeConfiguration<Agreement>
{
    public void Configure(EntityTypeBuilder<Agreement> builder)
    {
        builder
            .Property<long>("Id")
            .ValueGeneratedOnAdd();

        builder.HasKey("Id");
        
        builder
            .Property<DateTimeOffset>("CreatedOn")
            .HasDefaultValue(DateTimeOffset.UtcNow)
            .HasValueGenerator(typeof(DateTimeOffsetValueGenerator))
            .ValueGeneratedOnAdd();
        
        builder
            .Property<DateTimeOffset>("UpdatedOn")
            .HasDefaultValue(DateTimeOffset.UtcNow)
            .HasValueGenerator(typeof(DateTimeOffsetValueGenerator))
            .ValueGeneratedOnAddOrUpdate();

        builder.OwnsMany(a => a.DeliveryDays, dd =>
        {
            dd.Property(c => c.Value)
                .HasColumnName("DayOfWeek");
            
            dd.WithOwner().HasForeignKey("AgreementId");
            dd.Property<long>("Id");
            dd.HasKey("Id");
            
            dd.ToTable("Agreement_DeliveryDays");
        });
        
        builder
            .Property(c => c.Identifier)
            .HasMaxLength(Constants.IDS_LENGTH);
        
        builder
            .Property(c => c.CustomerIdentifier)
            .HasMaxLength(Constants.IDS_LENGTH);
        
        builder
            .Property(c => c.SupplierIdentifier)
            .HasMaxLength(Constants.IDS_LENGTH);
        
        builder
            .Property(c => c.CatalogIdentifier)
            .HasMaxLength(Constants.IDS_LENGTH);
        
        builder
            .HasIndex(c => c.Identifier)
            .IsUnique();
        
        builder.ToTable("Agreement");
    }
}