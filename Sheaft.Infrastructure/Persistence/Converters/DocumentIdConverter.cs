using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Converters;

internal class DocumentIdConverter : ValueConverter<DocumentId, string>
{
    public DocumentIdConverter()
        : base(
            v => v.Value,
            v => new DocumentId(v))
    {
    }
}