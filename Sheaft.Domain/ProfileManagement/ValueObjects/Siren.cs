namespace Sheaft.Domain.ProfileManagement;

public record Siren
{
    internal Siren(Siret siret)
    {
        Value = siret.Value[..9];
    }

    public string Value { get; }
}