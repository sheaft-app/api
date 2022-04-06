namespace Sheaft.Domain.OrderManagement;

public record DeliveryLine
{
    private DeliveryLine()
    {
    }

    internal DeliveryLine(string identifier, DeliveryLineKind lineKind, string reference, string name, Quantity quantity, Price unitPrice, VatRate vat)
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

    public static DeliveryLine CreateProductLine(ProductId identifier, ProductCode reference, ProductName name,
        Quantity quantity, ProductPrice unitPrice, VatRate vat)
    {
        return new DeliveryLine(identifier.Value, DeliveryLineKind.Product, reference.Value, name.Value, quantity, unitPrice, vat);
    }

    public static DeliveryLine CreateReturnableLine(ReturnableId identifier, ReturnableReference reference, ReturnableName name,
        Quantity quantity, Price unitPrice, VatRate vat)
    {
        return new DeliveryLine(identifier.Value, DeliveryLineKind.Returnable, reference.Value, name.Value, quantity, unitPrice, vat);
    }

    public static DeliveryLine CreateReturnedReturnableLine(ReturnableId identifier, ReturnableReference reference, ReturnableName name,
        Quantity quantity, Price unitPrice, VatRate vat)
    {
        return new DeliveryLine(identifier.Value, DeliveryLineKind.ReturnedReturnable, reference.Value, name.Value, quantity, unitPrice, vat);
    }

    public string Identifier { get; private set; }
    public DeliveryLineKind LineKind { get; private set; }
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