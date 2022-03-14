using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.AccountManagement;
using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;

namespace Sheaft.IntegrationTests.AccountManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class RegisterAccountCommandShould
{
    [Test]
    public async Task Register_Account()
    {
        var handler = InitHandler(new Dictionary<Username, EmailAddress>());

        var result = await handler.Handle(null,
            CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
    }

    [Test]
    public async Task Fail_If_EmailAddress_Already_Used()
    {
        var email = new EmailAddress("test@est.com");
        var handler = InitHandler(new Dictionary<Username, EmailAddress> {{new Username("username"),email}});

        var result = await handler.Handle(null,
            CancellationToken.None);
        
        Assert.IsTrue(result.IsFailure);
    }

    private RegisterAccountHandler InitHandler(Dictionary<Username, EmailAddress> emails)
    {
        var handler = new RegisterAccountHandler(null, null);
        return handler;
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618