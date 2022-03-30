﻿using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Converters;

internal class SupplierIdConverter : ValueConverter<SupplierId, string>
{
    public SupplierIdConverter()
        : base(
            v => v.Value,
            v => new SupplierId(v))
    {
    }
}