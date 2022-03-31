using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Converters;

internal class RetailerIdConverter : ValueConverter<RetailerId, string>
{
    public RetailerIdConverter()
        : base(
            v => v.Value,
            v => new RetailerId(v))
    {
    }
}