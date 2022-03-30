using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Converters;

internal class EmailAddressConverter : ValueConverter<EmailAddress, string>
{
    public EmailAddressConverter()
        : base(
            v => v.Value,
            v => new EmailAddress(v))
    {
    }
}