namespace Sheaft.Domain.SupplierManagement;

public record VatNumber
{
    internal VatNumber(Siren siren)
    {
        Key = (12 + 3 * (Convert.ToInt32(siren.Value) % 97)) % 97;
        Value = $"FR{Key}{siren.Value}";
    }

    public int Key { get; }
    public string Value { get; }
}