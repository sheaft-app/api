namespace Sheaft.Domain;

public record LineVatPrice
{
    public LineVatPrice(UnitPrice unitPrice, Quantity quantity, VatRate vat)
    {
        Value = Math.Round(unitPrice.Value * (vat.Value / 100) * quantity.Value, 2, MidpointRounding.ToEven);
    }

    public LineVatPrice(decimal value)
    {
        Value = Math.Round(value, 2, MidpointRounding.ToEven);
    }

    public decimal Value { get; }
}