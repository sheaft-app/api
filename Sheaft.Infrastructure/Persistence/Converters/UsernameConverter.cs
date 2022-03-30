using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sheaft.Domain.AccountManagement;

namespace Sheaft.Infrastructure.Persistence.Converters;

internal class UsernameConverter : ValueConverter<Username, string>
{
    public UsernameConverter()
        : base(
            v => v.Value,
            v => new Username(v))
    {
    }
}