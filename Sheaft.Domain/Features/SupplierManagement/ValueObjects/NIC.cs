namespace Sheaft.Domain.SupplierManagement;

public record NIC
{
    internal NIC(Siret siret)
    {
        Value = siret.Value[9..];
    }

    public string Value { get; }
}