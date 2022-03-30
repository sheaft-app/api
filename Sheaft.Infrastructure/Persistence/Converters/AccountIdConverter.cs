using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Converters;

internal class AccountIdConverter : ValueConverter<AccountId, string>
{
    public AccountIdConverter()
        : base(
            v => v.Value,
            v => new AccountId(v))
    {
    }
}