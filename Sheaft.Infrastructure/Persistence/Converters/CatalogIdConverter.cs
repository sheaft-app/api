using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Converters;

internal class CatalogIdConverter : ValueConverter<CatalogId, string>
{
    public CatalogIdConverter()
        : base(
            v => v.Value,
            v => new CatalogId(v))
    {
    }
}