using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;
using Sheaft.Infrastructure.AccountManagement;

namespace Sheaft.IntegrationTests.Helpers;

public static class AccountTests
{
    public static Account GetDefaultAccount(PasswordHasher hasher, string email = "test@test.com", string password = "P@ssword")
    {
        return new Account(new Username(email), new EmailAddress(email), HashedPassword.Create(password, hasher));
    }
}