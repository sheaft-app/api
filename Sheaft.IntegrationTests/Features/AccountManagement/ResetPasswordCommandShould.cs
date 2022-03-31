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
using Sheaft.Domain.AccountManagement;
using Sheaft.Infrastructure;
using Sheaft.Infrastructure.AccountManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.IntegrationTests.Fakes;
using Sheaft.IntegrationTests.Helpers;

namespace Sheaft.IntegrationTests.AccountManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class ResetPasswordCommandShould
{
    [Test]
    public async Task Reset_Account_Password()
    {
        var (account, handler) = InitHandler();

        var result = await handler.Handle(new ResetPasswordCommand(account.Identifier.Value, account.ResetPasswordInfo.Token, "newPassword", "newPassword"),
            CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNull(account.ResetPasswordInfo);

    }

    [Test]
    public async Task Fail_If_Identifier_Is_Invalid()
    {
        var (account, handler) = InitHandler();

        var result = await handler.Handle(new ResetPasswordCommand("invalidId", account.ResetPasswordInfo.Token, "newPassword", "newPassword"),
            CancellationToken.None);
        
        Assert.IsTrue(result.IsFailure);
    }

    [Test]
    public async Task Fail_If_Token_Is_Invalid()
    {
        var (account, handler) = InitHandler();

        var result = await handler.Handle(new ResetPasswordCommand(account.Identifier.Value, "invalidToken", "newPassword", "newPassword"),
            CancellationToken.None);
        
        Assert.IsTrue(result.IsFailure);
    }

    private (Account, ResetPasswordHandler) InitHandler()
    {
        var context = new FakeDbContextFactory().CreateContext();
        
        var username = "test@est.com";
        var password = "password";
        
        var hasher = new PasswordHasher("my_salt_value");
        var account = AccountTests.GetDefaultAccount(hasher, username, password);
        account.ForgotPassword(DateTimeOffset.UtcNow, 2);
        
        context.Accounts.Add(account);

        context.SaveChanges();
        
        var handler = new ResetPasswordHandler(
            new UnitOfWork(new FakeMediator(), context, new FakeLogger<UnitOfWork>()),
            hasher,
            new FakeLogger<ResetPasswordHandler>());
        
        return (account, handler);
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618