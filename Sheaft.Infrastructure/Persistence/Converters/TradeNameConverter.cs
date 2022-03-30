using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sheaft.Domain.SupplierManagement;

namespace Sheaft.Infrastructure.Persistence.Converters;

internal class TradeNameConverter : ValueConverter<TradeName, string>
{
    public TradeNameConverter()
        : base(
            v => v.Value,
            v => new TradeName(v))
    {
    }
}