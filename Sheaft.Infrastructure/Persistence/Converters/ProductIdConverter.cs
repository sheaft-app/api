using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Converters;

internal class ProductIdConverter : ValueConverter<ProductId, string>
{
    public ProductIdConverter()
        : base(
            v => v.Value,
            v => new ProductId(v))
    {
    }
}