using System.Text.RegularExpressions;

namespace Sheaft.Domain;

public record EmailAddress
{
    private EmailAddress(){}
    
    public const string EMAIL_REGEX =
        "^(?(\")(\".+?(?<!\\\\)\"@)|(([0-9a-z]((\\.(?!\\.))|[-!#\\$%&'\\*\\+/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9a-z])@))(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9a-z][-\\w]*[0-9a-z]*\\.)+[a-z0-9][\\-a-z0-9]{0,22}[a-z0-9]))$";
    public EmailAddress(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException("Email cannot be empty");

        if (!Regex.IsMatch(value, EMAIL_REGEX))
            throw new InvalidOperationException("Email is invalid");

        Value = value;
    }

    public string Value { get; }
}