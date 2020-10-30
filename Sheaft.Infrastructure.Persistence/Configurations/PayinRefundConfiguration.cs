﻿using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class PayinRefundConfiguration : IEntityTypeConfiguration<PayinRefund>
    {
        public void Configure(EntityTypeBuilder<PayinRefund> entity)
        {
            entity.Property<long>("PayinUid");
            entity.Property<long>("PurchaseOrderUid");

            entity.HasOne(c => c.PurchaseOrder).WithMany().HasForeignKey("PurchaseOrderUid").OnDelete(DeleteBehavior.NoAction);

            entity.HasIndex("PayinUid");
            entity.HasIndex("PurchaseOrderUid");
        }
    }
}
