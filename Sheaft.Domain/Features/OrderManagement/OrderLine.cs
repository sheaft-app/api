namespace Sheaft.Domain.OrderManagement;

public record OrderLine
{
    private OrderLine()
    {
    }

    public OrderLine(ProductId productIdentifier, ProductCode code, ProductName name, Quantity quantity, Price unitPrice, VatRate vat)
    {
        ProductIdentifier = productIdentifier;
        Code = code;
        Name = name;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Vat = vat;
        TotalPrice = GetTotalPrice(Quantity, UnitPrice);;
    }
    public ProductId ProductIdentifier { get; private set; }
    public ProductCode Code { get; private set; }
    public ProductName Name { get; private set; }
    public Quantity Quantity { get; private set; }
    public Price UnitPrice { get; private set; }
    public VatRate Vat { get; private set; }
    public Price TotalPrice { get; private set; }

    public Result UpdateQuantity(Quantity quantity)
    {
        Quantity = quantity;
        TotalPrice = GetTotalPrice(Quantity, UnitPrice);
        return Result.Success();
    }
    
    private static Price GetTotalPrice(Quantity quantity, Price unitPrice)
    {
        return new Price(unitPrice.Value * quantity.Value, unitPrice.Currency);
    }
}

public record ProductsQuantities(ProductId ProductIdentifier, Quantity Quantity);