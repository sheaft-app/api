namespace Sheaft.Domain;

public record LinePrice
{
    private LinePrice(){}
    
    public LinePrice(UnitPrice unitPrice, VatRate vat, Quantity quantity)
    {
        UnitPrice = unitPrice;
        WholeSalePrice = new LineWholeSalePrice(UnitPrice, quantity);
        VatPrice = new LineVatPrice(UnitPrice, quantity, vat);
        OnSalePrice = new LineOnSalePrice(UnitPrice, quantity, vat);
    }

    public UnitPrice UnitPrice { get; }
    public LineWholeSalePrice WholeSalePrice { get; }
    public LineVatPrice VatPrice { get; }
    public LineOnSalePrice OnSalePrice { get; }
}