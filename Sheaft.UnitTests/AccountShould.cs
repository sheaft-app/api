using System.Linq;
using NUnit.Framework;
using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;
using Sheaft.Domain.AccountManagement.Events;
using Sheaft.Domain.AccountManagement.Services;
using Sheaft.Domain.AccountManagement.ValueObjects;
using Sheaft.Infrastructure.Services;

namespace Sheaft.UnitTests;

public class AccountShould
{
    private IPasswordHasher _passwordHasher;

    [SetUp]
    public void Setup()
    {
        _passwordHasher = new PasswordHasher();
    }

    [Test]
    public void Return_Email_As_Username()
    {
        var account = InitTestAccount();

        Assert.IsNotNull(account);
        Assert.AreEqual(account.Email.Email, account.Username);
    }

    [Test]
    public void Not_Raise_EmailChanged_Event_If_NewEmail_Is_Identical()
    {
        var account = InitTestAccount();
        var newEmail = new NewEmail("test@test.com", "test@test.com");

        account.ChangeEmail(newEmail);

        Assert.AreEqual(newEmail.Email, account.Email.Email);
        Assert.AreEqual(0, account.DomainEvents.Count(e => e is EmailChanged));
    }

    [Test]
    public void Raise_PasswordChanged_Event_When_Changing_Password()
    {
        var account = InitTestAccount();
        account.ChangePassword(new ChangePassword("aaaaaaaa",  new NewPassword("bbbbbbbb", "bbbbbbbb")), _passwordHasher);

        Assert.AreEqual(1, account.DomainEvents.Count(e => e is PasswordChanged));
    }

    [Test]
    public void Throw_Exception_When_Changing_Password_If_New_Password_Is_Same_As_Old_One()
    {
        var account = InitTestAccount();
        Assert.That(() => account.ChangePassword(new ChangePassword("aaaaaaaa",  new NewPassword("aaaaaaaa", "aaaaaaaa")), _passwordHasher), Throws.InvalidOperationException);
    }

    [Test]
    public void Throw_Exception_When_Changing_Password_If_Old_Password_Is_Wrong()
    {
        var account = InitTestAccount();
        Assert.That(() => account.ChangePassword(new ChangePassword("z",  new NewPassword("eeeeeeee", "eeeeeeee")), _passwordHasher), Throws.InvalidOperationException);
    }

    [Test]
    public void Login_Successfully_If_Password_Is_Valid()
    {
        var account = InitTestAccount();
        var isAllowedToLogin = account.Login("aaaaaaaa", _passwordHasher);
        
        Assert.IsTrue(isAllowedToLogin);
    }

    [Test]
    public void Fail_Login_If_Password_Is_Invalid()
    {
        var account = InitTestAccount();
        var isAllowedToLogin = account.Login("b", _passwordHasher);
        
        Assert.IsFalse(isAllowedToLogin);
    }

    private Account InitTestAccount(string? email = null)
    {
        return new Account(new EmailAddress(email ?? "test@test.com"),  HashedPassword.Create(new Password("aaaaaaaa"), _passwordHasher));
    }
}