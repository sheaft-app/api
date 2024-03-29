﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class ReturnableConfiguration : IEntityTypeConfiguration<Returnable>
    {
        private readonly bool _isAdmin;

        public ReturnableConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }

        public void Configure(EntityTypeBuilder<Returnable> entity)
        {
            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn);
            entity.Property(c => c.RowVersion).IsRowVersion();

            if (!_isAdmin)
                entity.HasQueryFilter(p => !p.RemovedOn.HasValue);

            entity.Property(c => c.Kind).HasDefaultValue(ReturnableKind.Container);

            entity.Property(o => o.Name).IsRequired();
            entity.Property(o => o.WholeSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Vat).HasColumnType("decimal(10,2)");

            entity.Ignore(o => o.VatPrice);
            entity.Ignore(o => o.OnSalePrice);
            
            entity.HasKey(c => c.Id);
            entity.ToTable("Returnables");
        }
    }
}