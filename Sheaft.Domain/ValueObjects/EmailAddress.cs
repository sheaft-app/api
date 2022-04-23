using System.Text.RegularExpressions;

namespace Sheaft.Domain;

public record EmailAddress
{
    private EmailAddress(){}
    
    public const string EMAIL_REGEX =
        "^(?(\")(\".+?(?<!\\\\)\"@)|(([0-9A-z]((\\.(?!\\.))|[-!#\\$%&'\\*\\+/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9A-z])@))(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9A-z][-\\w]*[0-9A-z]*\\.)+[A-z0-9][\\-A-z0-9]{0,22}[A-z0-9]))$";

    public const int MAXLENGTH = 254;

    public EmailAddress(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException("Email cannot be empty");

        if (!Regex.IsMatch(value, EMAIL_REGEX))
            throw new InvalidOperationException("Email is invalid");
        
        if (value.Length > MAXLENGTH)
            throw new InvalidOperationException("Email length is invalid (max: 254 characters)");

        Value = value.ToLowerInvariant();
    }

    public string Value { get; }
}