using System.Text.RegularExpressions;

namespace Sheaft.Domain;

public record Siret
{
    private Siret(){}
    public Siret(string value)
    {
        if(string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException(nameof(value));
            
        if (value.Length != 14)
            throw new InvalidOperationException("Siret must be 14 characters.");

        if (!Regex.IsMatch(value, "[0-9]"))
            throw new InvalidOperationException("Siret must contains only digits");

        Value = value;
    }

    public string Value { get; }
    public VatNumber VatNumber => new VatNumber(Siren);
    public NIC NIC => new NIC(this);
    public Siren Siren => new Siren(this);
}