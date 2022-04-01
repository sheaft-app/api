using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Converters;

internal class OrderIdConverter : ValueConverter<OrderId, string>
{
    public OrderIdConverter()
        : base(
            v => v.Value,
            v => new OrderId(v))
    {
    }
}