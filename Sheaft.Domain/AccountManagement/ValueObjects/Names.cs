namespace Sheaft.Domain.AccountManagement;

public record Name
{
    public Name(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(value));

        Value = value;
    }

    public string Value { get; }
}

public record CompanyName(string Value) : Name(Value);
public record LegalName(string Value) : Name(Value);