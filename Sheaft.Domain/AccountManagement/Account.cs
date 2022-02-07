using System;
using Sheaft.Domain.AccountManagement.Events;
using Sheaft.Domain.AccountManagement.Services;
using Sheaft.Domain.AccountManagement.ValueObjects;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.AccountManagement;

public class Account : AggregateRoot
{
    public Account(EmailAddress email, HashedPassword password)
    {
        Email = email;
        Password = password;
    }

    public string Username => Email.Email;
    public EmailAddress Email { get; private set; }
    public HashedPassword Password { get; private set; }

    public void ChangeEmail(NewEmail newEmail)
    {
        if (Email.Email == newEmail.Email)
            return;

        var oldEmail = Email.Email;
        Email = newEmail;

        RaiseEvent(new EmailChanged(Identifier, oldEmail, Email.Email));
    }

    public void ChangePassword(ChangePassword changePassword, IPasswordHasher hasher)
    {
        var oldPasswordHashed = HashedPassword.Create(changePassword.OldPassword, hasher);
        if (oldPasswordHashed.Hash != Password.Hash)
            throw new InvalidOperationException("The current password is invalid.");

        var newPasswordHashed = HashedPassword.Create(changePassword.NewPassword, hasher);
        if (newPasswordHashed.Hash == Password.Hash)
            throw new InvalidOperationException("The new password must be different from the old one.");

        Password = newPasswordHashed;
        RaiseEvent(new PasswordChanged(Identifier));
    }

    public bool Login(string password, IPasswordHasher hasher)
    {
        var hashedPassword = HashedPassword.Create(password, hasher);
        if (hashedPassword.Hash != Password.Hash)
            return false;
        
        RaiseEvent(new AccountLoggedIn(Identifier));
        return true;
    }
}