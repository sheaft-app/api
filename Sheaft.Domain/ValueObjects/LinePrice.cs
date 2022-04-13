namespace Sheaft.Domain;

public record LinePrice
{
    public LinePrice(UnitPrice unitPrice, VatRate vat, Quantity quantity)
    {
        Quantity = quantity;
        UnitPrice = unitPrice;
        WholeSalePrice = new LineWholeSalePrice(UnitPrice, Quantity);
        Vat = vat;
        VatPrice = new LineVatPrice(UnitPrice, Quantity, Vat);
        OnSalePrice = new LineOnSalePrice(UnitPrice, Quantity, Vat);
    }

    public Quantity Quantity { get; }
    public UnitPrice UnitPrice { get; }
    public LineWholeSalePrice WholeSalePrice { get; }
    public LineVatPrice VatPrice { get; }
    public LineOnSalePrice OnSalePrice { get; }
    public VatRate Vat { get; }
}