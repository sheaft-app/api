namespace Sheaft.Domain.OrderManagement;

public record OrderLine
{
    private OrderLine()
    {
    }

    private OrderLine(string identifier, OrderLineKind lineKind, string reference, string name, Quantity quantity, Price unitPrice, VatRate vat)
    {
        Identifier = identifier;
        LineKind = lineKind;
        Reference = reference;
        Name = name;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Vat = vat;
        TotalPrice = GetTotalPrice(Quantity, UnitPrice);
    }

    public static OrderLine CreateProductLine(ProductId productIdentifier, ProductCode reference, ProductName name,
        Quantity quantity, ProductPrice unitPrice, VatRate vat)
    {
        return new OrderLine(productIdentifier.Value, OrderLineKind.Product, reference.Value, name.Value, quantity, unitPrice, vat);
    }

    public static OrderLine CreateReturnableLine(ReturnableId returnableIdentifier, ReturnableReference reference, ReturnableName name,
        Quantity quantity, Price unitPrice, VatRate vat)
    {
        return new OrderLine(returnableIdentifier.Value, OrderLineKind.Returnable, reference.Value, name.Value, quantity, unitPrice, vat);
    }

    public string Identifier { get; private set; }
    public OrderLineKind LineKind { get; private set; }
    public string Reference { get; private set; }
    public string Name { get; private set; }
    public Quantity Quantity { get; private set; }
    public Price UnitPrice { get; private set; }
    public VatRate Vat { get; private set; }
    public Price TotalPrice { get; private set; }

    private static Price GetTotalPrice(Quantity quantity, Price? unitPrice)
    {
        return new Price((unitPrice?.Value ?? 0) * quantity.Value, unitPrice?.Currency ?? Currency.Euro);
    }
}

public record ProductsQuantities(ProductId ProductIdentifier, Quantity Quantity);