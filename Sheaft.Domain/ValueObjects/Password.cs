using System.Text.RegularExpressions;

namespace Sheaft.Domain;

public record Password
{
    public const int MIN_LENGTH = 8;
    public Password(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new InvalidOperationException("Password is required.");
        
        if (password.Length < MIN_LENGTH)
            throw new InvalidOperationException($"Password is too short (minimum {MIN_LENGTH} characters)");
        
        if (!Regex.IsMatch(password, "[\\w\\W]+"))
            throw new InvalidOperationException("Password contains invalid characters");

        Value = password;
    }

    public string Value { get; }
}