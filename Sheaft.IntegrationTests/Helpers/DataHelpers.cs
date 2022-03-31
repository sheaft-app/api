using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;
using Sheaft.Domain.CustomerManagement;
using Sheaft.Domain.SupplierManagement;
using Sheaft.Infrastructure.AccountManagement;

namespace Sheaft.IntegrationTests.Helpers;

public static class DataHelpers
{
    public static Account GetDefaultAccount(PasswordHasher hasher, string email = "test@test.com", string password = "P@ssword")
    {
        return new Account(new Username(email), new EmailAddress(email), HashedPassword.Create(password, hasher));
    }

    public static Supplier GetDefaultSupplier(AccountId accountIdentifier, string emailAddress = "test@test.com")
    {
        return new Supplier(new TradeName("trade"), new EmailAddress(emailAddress), new PhoneNumber("0664566565"),
            new Legal(new CorporateName("le"), new Siret("15932477173006"), new LegalAddress("", null, "", "")), null,
            accountIdentifier);
    }

    public static Customer GetDefaultCustomer(AccountId accountIdentifier, string emailAddress = "test@test.com")
    {
        return new Customer(new TradeName("trade"), new EmailAddress(emailAddress), new PhoneNumber("0664566565"),
            new Legal(new CorporateName("le"), new Siret("15932477173006"), new LegalAddress("", null, "", "")), null,
            accountIdentifier);
    }
}