using System;
using System.Linq;
using NUnit.Framework;
using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;
using Sheaft.Infrastructure;
using Sheaft.Infrastructure.AccountManagement;

namespace Sheaft.UnitTests.Domain;

public class AccountShould
{
    private IPasswordHasher _passwordHasher;
    private ISecurityTokensProvider _securityTokenProviders;

    [SetUp]
    public void Setup()
    {
        _passwordHasher = new PasswordHasher("my_salt_value");
        _securityTokenProviders = new SecurityTokensProvider(new JwtSettings
        {
            Issuer = "http://localhost",
            Secret = "my_secret_is_very_long",
        }, new SecuritySettings
        {
            AccessTokenExpirationInMinutes = 5,
            RefreshTokenExpirationInMinutes = 30
        });
    }

    [Test]
    public void Fail_ChangeEmail_If_NewEmail_Is_Identical()
    {
        var account = InitTestAccount();
        var newEmail = new NewEmailAddress("test@test.com", "test@test.com");

        var result = account.ChangeEmail(newEmail);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(0, account.Events.Count(e => e.Value is EmailChanged));
    }

    [Test]
    public void Raise_PasswordChanged_Event_When_Changing_Password()
    {
        var account = InitTestAccount();

        var result = account.ChangePassword(new ChangePassword("aaaaaaaa", "bbbbbbbb", "bbbbbbbb"), _passwordHasher);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(1, account.Events.Count(e => e.Value is PasswordChanged));
    }

    [Test]
    public void Fail_When_Changing_Password_If_New_Password_Is_Same_As_Old_One()
    {
        var account = InitTestAccount();
        var result = account.ChangePassword(new ChangePassword("aaaaaaaa", "aaaaaaaa", "aaaaaaaa"), _passwordHasher);

        Assert.IsTrue(result.IsFailure);
    }

    [Test]
    public void Fail_When_Changing_Password_If_Old_Password_Is_Wrong()
    {
        var account = InitTestAccount();
        var result = account.ChangePassword(new ChangePassword("z", "eeeeeeee", "eeeeeeee"), _passwordHasher);

        Assert.IsTrue(result.IsFailure);
    }

    [Test]
    public void Login_Successfully_If_Password_Is_Valid()
    {
        var account = InitTestAccount();
        var result = account.Login("aaaaaaaa", _passwordHasher, _securityTokenProviders, new Profile("test", "name", ProfileKind.Supplier));

        Assert.IsTrue(result.IsSuccess);
        Assert.IsTrue(account.Events.Any(e => e.Value is AccountLoggedIn));
    }

    [Test]
    public void Generate_A_Refresh_Token_On_Successfull_Login()
    {
        var account = InitTestAccount();
        var result = account.Login("aaaaaaaa", _passwordHasher, _securityTokenProviders, new Profile("test", "name", ProfileKind.Supplier));

        Assert.IsTrue(result.IsSuccess);
    }

    [Test]
    public void Invalidate_Old_Refresh_Tokens_On_Successfull_Login()
    {
        var account = InitTestAccount();
        var result = account.Login("aaaaaaaa", _passwordHasher, _securityTokenProviders, new Profile("test", "name", ProfileKind.Supplier));

        Assert.IsTrue(result.IsSuccess);
    }

    [Test]
    public void Fail_If_Password_Is_Invalid()
    {
        var account = InitTestAccount();
        var result = account.Login("b", _passwordHasher, _securityTokenProviders, new Profile("test", "name", ProfileKind.Supplier));

        Assert.IsTrue(result.IsFailure);
    }

    [Test]
    public void Generate_Valid_ResetPasswordToken_When_ForgotPassword()
    {
        var account = InitTestAccount();
        var result = account.ForgotPassword(DateTimeOffset.UtcNow, 24);

        Assert.IsTrue(result.IsSuccess);
        Assert.IsTrue(account.Events.Any(e => e.Value is PasswordForgotten));

        Assert.IsNotNull(account.ResetPasswordInfo.Token);
        Assert.IsTrue(account.ResetPasswordInfo.ExpiresOn > DateTimeOffset.UtcNow.AddMinutes(24 * 60 - 1));
    }

    [Test]
    public void Complete_ResetPassword_When_Token_Is_Valid()
    {
        var account = InitTestAccount();
        account.ForgotPassword(DateTimeOffset.UtcNow, 24);

        var result = account.ResetPassword(account.ResetPasswordInfo.Token, new NewPassword("P@sswOrd", "P@sswOrd"),
            _passwordHasher);

        Assert.IsTrue(result.IsSuccess);
        Assert.IsTrue(account.Events.Any(e => e.Value is PasswordReset));

        Assert.IsNull(account.ResetPasswordInfo);
    }

    [Test]
    public void Fail_ResetPassword_When_Token_Is_Invalid()
    {
        var account = InitTestAccount();
        account.ForgotPassword(DateTimeOffset.UtcNow, 24);

        var result = account.ResetPassword("invalid token", new NewPassword("P@sswOrd", "P@sswOrd"), _passwordHasher);

        Assert.IsTrue(result.IsFailure);
    }

    [Test]
    public void Fail_ResetPassword_When_No_ResetToken()
    {
        var account = InitTestAccount();

        var result = account.ResetPassword("invalid token", new NewPassword("P@sswOrd", "P@sswOrd"), _passwordHasher);

        Assert.IsTrue(result.IsFailure);
    }

    [Test]
    public void Fail_ResetPassword_When_Token_Is_Expired()
    {
        var account = InitTestAccount();
        account.ForgotPassword(DateTimeOffset.UtcNow.AddDays(-2), 24);

        var result = account.ResetPassword(account.ResetPasswordInfo.Token, new NewPassword("P@sswOrd", "P@sswOrd"),
            _passwordHasher);

        Assert.IsTrue(result.IsFailure);
    }

    private Account InitTestAccount(string? email = null)
    {
        return new Account(
            new Username("testusername"),
            new EmailAddress(email ?? "test@test.com"),
            HashedPassword.Create(new Password("aaaaaaaa"), _passwordHasher),
            new Firstname("first"),
            new Lastname("last"));
    }
}