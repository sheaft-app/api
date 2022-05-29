using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Converters;

internal class OwnerIdConverter : ValueConverter<OwnerId, string>
{
    public OwnerIdConverter()
        : base(
            v => v.Value,
            v => new OwnerId(v))
    {
    }
}