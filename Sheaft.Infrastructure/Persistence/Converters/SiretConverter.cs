using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sheaft.Domain.SupplierManagement;

namespace Sheaft.Infrastructure.Persistence.Converters;

internal class SiretConverter : ValueConverter<Siret, string>
{
    public SiretConverter()
        : base(
            v => v.Value,
            v => new Siret(v))
    {
    }
}