using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Sheaft.Application;
using Sheaft.Application.AccountManagement;
using Sheaft.Domain;
using Sheaft.Infrastructure;
using Sheaft.Infrastructure.AccountManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.UnitTests.Fakes;
using Sheaft.UnitTests.Helpers;

namespace Sheaft.UnitTests.AccountManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class ForgotPasswordCommandShould
{
    [Test]
    public async Task Generate_Reset_Password_Info()
    {
        var (email, context, handler) = InitHandler();

        var result = await handler.Handle(new ForgotPasswordCommand(email),
            CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);

        var account = context.Accounts.FirstOrDefault(c => c.Email == new EmailAddress(email));
        Assert.IsNotNull(account?.ResetPasswordInfo);
        Assert.IsTrue(account.ResetPasswordInfo.ExpiresOn > DateTimeOffset.UtcNow);
    }

    [Test]
    public async Task Succeed_If_Email_Not_Found()
    {
        var (email, context, handler) = InitHandler();

        var result = await handler.Handle(new ForgotPasswordCommand("test@bad.com"),
            CancellationToken.None);
        
        Assert.IsTrue(result.IsSuccess);
    }

    private (string, AppDbContext, ForgotPasswordHandler) InitHandler()
    {
        var context = new FakeDbContextFactory().CreateContext();
        
        var username = "test@est.com";
        var password = "password";
        
        var hasher = new PasswordHasher("my_salt_value");
        context.Accounts.Add(DataHelpers.GetDefaultAccount(hasher, username, password));

        context.SaveChanges();
        
        var handler = new ForgotPasswordHandler(
            new UnitOfWork(new FakeMediator(), context, new FakeLogger<UnitOfWork>()),
            new SecuritySettings{
                AccessTokenExpirationInMinutes = 5,
                RefreshTokenExpirationInMinutes = 5,
                ResetPasswordTokenValidityInHours = 5
            }, 
            new FakeLogger<ForgotPasswordHandler>());
        
        return (username, context, handler);
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618