using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;
using Sheaft.Domain.ProfileManagement;
using Sheaft.Infrastructure.AccountManagement;

namespace Sheaft.IntegrationTests.AccountManagement;

public static class AccountTestsHelper
{
    public static Account GetDefaultAccount(PasswordHasher hasher, string email = "test@test.com", string password = "P@ssword", string siret = "15932477173006")
    {
        return new Account(new Username(email), new EmailAddress(email),
            HashedPassword.Create(password, hasher), new Profile(new CompanyName("name"), new EmailAddress("last@test.com"), new PhoneNumber("0600125412"), new Legal(new LegalName("test"), new Siret(siret), new Address("line1", "line2", "73410", "Albens")), new UserInfo("firstname", "lastname")));
    }
}