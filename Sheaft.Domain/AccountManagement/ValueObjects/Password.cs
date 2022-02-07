using System;
using System.Text.RegularExpressions;

namespace Sheaft.Domain.AccountManagement.ValueObjects;

public record Password
{
    public const int MIN_LENGTH = 8;
    public Password(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentNullException(nameof(password));
        
        if (password.Length < MIN_LENGTH)
            throw new InvalidOperationException($"Password is too short (minimum {MIN_LENGTH} characters)");
        
        if (!Regex.IsMatch(password, "[\\w\\W]+"))
            throw new InvalidOperationException("Password is invalid");

        Value = password;
    }

    public string Value { get; }
}