namespace Sheaft.Domain;

public record CorporateName
{
    public const int MAXLENGTH = 100;

    public CorporateName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(value));

        Value = value;
    }

    public string Value { get; }
}