namespace Sheaft.Domain.OrderManagement;

public record OrderLine
{
    private OrderLine()
    {
    }

    internal OrderLine(string identifier, OrderLineKind lineKind, string reference, string name, OrderedQuantity quantity, UnitPrice unitPrice, VatRate vat)
    {
        Identifier = identifier;
        LineKind = lineKind;
        Reference = reference;
        Name = name;
        PriceInfo = new LinePrice(unitPrice, vat, quantity);
        Quantity = quantity;
        Vat = vat;
    }

    public static OrderLine CreateProductLine(ProductId productIdentifier, ProductReference reference, ProductName name,
        OrderedQuantity quantity, ProductUnitPrice unitPrice, VatRate vat)
    {
        return new OrderLine(productIdentifier.Value, OrderLineKind.Product, reference.Value, name.Value, quantity, unitPrice, vat);
    }

    public static OrderLine CreateReturnableLine(ReturnableId returnableIdentifier, ReturnableReference reference, ReturnableName name,
        OrderedQuantity quantity, UnitPrice unitPrice, VatRate vat)
    {
        return new OrderLine(returnableIdentifier.Value, OrderLineKind.Returnable, reference.Value, name.Value, quantity, unitPrice, vat);
    }

    public string Identifier { get; private set; }
    public OrderLineKind LineKind { get; private set; }
    public string Reference { get; private set; }
    public string Name { get; private set; }
    public LinePrice PriceInfo { get; private set; }
    public Quantity Quantity { get; }
    public VatRate Vat { get; }
}