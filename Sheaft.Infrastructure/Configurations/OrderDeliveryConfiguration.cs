﻿using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure
{
    public class OrderDeliveryConfiguration : IEntityTypeConfiguration<OrderDelivery>
    {
        public void Configure(EntityTypeBuilder<OrderDelivery> entity)
        {
            entity.Property<long>("OrderUid");

            entity.HasKey("OrderUid", "Id");

            entity.ToTable("OrderDeliveries");
        }
    }
}
