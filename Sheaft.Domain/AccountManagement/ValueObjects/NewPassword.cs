using System;

namespace Sheaft.Domain.AccountManagement.ValueObjects;

public record NewPassword : Password
{
    public NewPassword(string password, string confirm)
        :base(password)
    {
        if (password != confirm)
            throw new InvalidOperationException("Password and confirmation are different");
    }
}