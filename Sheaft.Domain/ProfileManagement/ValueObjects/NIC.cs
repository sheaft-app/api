namespace Sheaft.Domain.ProfileManagement;

public record NIC
{
    internal NIC(Siret siret)
    {
        Value = siret.Value[9..];
    }

    public string Value { get; }
}