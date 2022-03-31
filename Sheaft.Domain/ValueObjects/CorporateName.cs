namespace Sheaft.Domain;

public record CorporateName
{
    public CorporateName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(value));

        Value = value;
    }

    public string Value { get; }
}