using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sheaft.Domain.SupplierManagement;

namespace Sheaft.Infrastructure.Persistence.Converters;

internal class CorporateNameConverter : ValueConverter<CorporateName, string>
{
    public CorporateNameConverter()
        : base(
            v => v.Value,
            v => new CorporateName(v))
    {
    }
}