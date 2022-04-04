using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Converters;

internal class ReturnableIdConverter : ValueConverter<ReturnableId, string>
{
    public ReturnableIdConverter()
        : base(
            v => v.Value,
            v => new ReturnableId(v))
    {
    }
}