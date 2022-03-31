using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Converters;

internal class AgreementIdConverter : ValueConverter<AgreementId, string>
{
    public AgreementIdConverter()
        : base(
            v => v.Value,
            v => new AgreementId(v))
    {
    }
}