using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Converters;

internal class CustomerIdConverter : ValueConverter<CustomerId, string>
{
    public CustomerIdConverter()
        : base(
            v => v.Value,
            v => new CustomerId(v))
    {
    }
}