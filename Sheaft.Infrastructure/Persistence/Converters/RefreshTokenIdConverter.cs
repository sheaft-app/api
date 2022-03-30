using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Converters;

internal class RefreshTokenIdConverter : ValueConverter<RefreshTokenId, string>
{
    public RefreshTokenIdConverter()
        : base(
            v => v.Value,
            v => new RefreshTokenId(v))
    {
    }
}