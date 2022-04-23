namespace Sheaft.Domain;

public record TradeName
{
    public const int MAXLENGTH = 100;

    public TradeName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(value));

        Value = value;
    }

    public string Value { get; }
}