using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Converters;

internal class InvoiceIdConverter : ValueConverter<InvoiceId, string>
{
    public InvoiceIdConverter()
        : base(
            v => v.Value,
            v => new InvoiceId(v))
    {
    }
}