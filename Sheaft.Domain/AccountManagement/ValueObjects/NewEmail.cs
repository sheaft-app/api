using System;

namespace Sheaft.Domain.AccountManagement.ValueObjects;

public record NewEmail : EmailAddress
{
    public NewEmail(string newEmail, string confirmEmailAddress)
        : base(newEmail)
    {
        if (newEmail != confirmEmailAddress)
            throw new InvalidOperationException("Email and confirmation are different");
    }
}