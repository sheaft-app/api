using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Converters;

internal class BatchIdConverter : ValueConverter<BatchId, string>
{
    public BatchIdConverter()
        : base(
            v => v.Value,
            v => new BatchId(v))
    {
    }
}