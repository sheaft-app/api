namespace Sheaft.Domain.AccountManagement.ValueObjects;

public record Siren
{
    internal Siren(Siret siret)
    {
        Value = siret.Value[..9];
    }

    public string Value { get; }
}