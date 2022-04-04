using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Converters;

internal class DeliveryIdConverter : ValueConverter<DeliveryId, string>
{
    public DeliveryIdConverter()
        : base(
            v => v.Value,
            v => new DeliveryId(v))
    {
    }
}