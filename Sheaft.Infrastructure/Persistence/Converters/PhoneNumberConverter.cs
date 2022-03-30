using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Converters;

internal class PhoneNumberConverter : ValueConverter<PhoneNumber, string>
{
    public PhoneNumberConverter()
        : base(
            v => v.Value,
            v => new PhoneNumber(v))
    {
    }
}