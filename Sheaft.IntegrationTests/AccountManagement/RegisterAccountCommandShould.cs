using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.AccountManagement;
using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;
using Sheaft.Infrastructure;
using Sheaft.Infrastructure.AccountManagement;
using Sheaft.IntegrationTests.Fakes;

namespace Sheaft.IntegrationTests.AccountManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class RegisterAccountCommandShould
{
    [Test]
    public async Task Register_Account()
    {
        var handler = InitHandler(new List<EmailAddress>());

        var result = await handler.Handle(GetCommand("super@est.com"),
            CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
    }

    [Test]
    public async Task Fail_If_EmailAddress_Already_Used()
    {
        var email = new EmailAddress("test@est.com");
        var handler = InitHandler(new List<EmailAddress> {email});

        var result = await handler.Handle(GetCommand(email.Value),
            CancellationToken.None);
        
        Assert.IsTrue(result.IsFailure);
    }

    private RegisterAccountCommand GetCommand(string email)
    {
        return new RegisterAccountCommand(email, "password", "password", "test", "test@test.com", "0646654532", "test", "15932477173006", "line", "line", "zip", "city", "first", "last");
    }

    private RegisterAccountHandler InitHandler(List<EmailAddress> emails)
    {
        var password = "password";
        
        var hasher = new PasswordHasher("this_salt_is_awesome");
        var context = new FakeDbContextFactory().CreateContext();

        foreach (var emailAddress in emails)
        {
            var account = AccountTestsHelper.GetDefaultAccount(hasher, emailAddress.Value, password);
            context.Accounts.Add(account);
        }
        
        context.SaveChanges();
        
        var handler = new RegisterAccountHandler(new UnitOfWork(new FakeMediator(), context, new FakeLogger<UnitOfWork>()), new CreateAccount(new ValidateUniqueness(context), new PasswordHasher("this_is_super_salt_sample")));
        return handler;
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618