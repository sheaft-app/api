using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;
using Sheaft.Domain.SupplierManagement;

namespace Sheaft.Infrastructure.Persistence.Converters;

public class SupplierIdConverter : ValueConverter<SupplierId, string>
{
    public SupplierIdConverter()
        : base(
            v => v.Value,
            v => new SupplierId(v))
    {
    }
}
public class AccountIdConverter : ValueConverter<AccountId, string>
{
    public AccountIdConverter()
        : base(
            v => v.Value,
            v => new AccountId(v))
    {
    }
}
public class RefreshTokenIdConverter : ValueConverter<RefreshTokenId, string>
{
    public RefreshTokenIdConverter()
        : base(
            v => v.Value,
            v => new RefreshTokenId(v))
    {
    }
}
public class UsernameConverter : ValueConverter<Username, string>
{
    public UsernameConverter()
        : base(
            v => v.Value,
            v => new Username(v))
    {
    }
}
public class EmailAddressConverter : ValueConverter<EmailAddress, string>
{
    public EmailAddressConverter()
        : base(
            v => v.Value,
            v => new EmailAddress(v))
    {
    }
}
public class PhoneNumberConverter : ValueConverter<PhoneNumber, string>
{
    public PhoneNumberConverter()
        : base(
            v => v.Value,
            v => new PhoneNumber(v))
    {
    }
}
public class TradeNameConverter : ValueConverter<TradeName, string>
{
    public TradeNameConverter()
        : base(
            v => v.Value,
            v => new TradeName(v))
    {
    }
}
public class CorporateNameConverter : ValueConverter<CorporateName, string>
{
    public CorporateNameConverter()
        : base(
            v => v.Value,
            v => new CorporateName(v))
    {
    }
}
public class SiretConverter : ValueConverter<Siret, string>
{
    public SiretConverter()
        : base(
            v => v.Value,
            v => new Siret(v))
    {
    }
}