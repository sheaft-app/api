using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Sheaft.Infrastructure
{
    public class PurchaseTransactionConfiguration : IEntityTypeConfiguration<PurchaseTransaction>
    {
        public void Configure(EntityTypeBuilder<PurchaseTransaction> entity)
        {
            entity.Property<long>("PurchaseOrderUid");

            entity.HasIndex("PurchaseOrderUid");
        }
    }
}
