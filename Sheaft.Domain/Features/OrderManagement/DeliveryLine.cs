namespace Sheaft.Domain.OrderManagement;

public record DeliveryLine
{
    private DeliveryLine()
    {
    }

    internal DeliveryLine(string identifier, DeliveryLineKind lineKind, string reference, string name, Quantity quantity, UnitPrice unitPrice, VatRate vat, DeliveryOrder? order = null, IEnumerable<BatchId>? batches = null)
    {
        Identifier = identifier;
        LineKind = lineKind;
        Reference = reference;
        Name = name;
        PriceInfo = new LinePrice(unitPrice, vat, quantity);
        Quantity = quantity;
        Vat = vat;
        Order = order;
        CreatedOn = DateTimeOffset.UtcNow;
        Batches = batches?.Select(b => new DeliveryBatch(b)).ToList() ?? new List<DeliveryBatch>();
    }

    public static DeliveryLine CreateProductLine(ProductId identifier, ProductReference reference, ProductName name,
        Quantity quantity, ProductUnitPrice unitPrice, VatRate vat, DeliveryOrder order, IEnumerable<BatchId>? batches)
    {
        return new DeliveryLine(identifier.Value, DeliveryLineKind.Product, reference.Value, name.Value, quantity, unitPrice, vat, order, batches);
    }

    public static DeliveryLine CreateReturnableLine(ReturnableId identifier, ReturnableReference reference, ReturnableName name,
        Quantity quantity, UnitPrice unitPrice, VatRate vat, DeliveryOrder order)
    {
        return new DeliveryLine(identifier.Value, DeliveryLineKind.Returnable, reference.Value, name.Value, quantity, unitPrice, vat, order);
    }

    public static DeliveryLine CreateReturnedReturnableLine(ReturnableId identifier, ReturnableReference reference, ReturnableName name,
        Quantity quantity, UnitPrice unitPrice, VatRate vat)
    {
        return new DeliveryLine(identifier.Value, DeliveryLineKind.ReturnedReturnable, reference.Value, name.Value, quantity, unitPrice, vat);
    }

    public string Identifier { get; private set; }
    public DeliveryLineKind LineKind { get; private set; }
    public string Reference { get; private set; }
    public string Name { get; private set; }
    public LinePrice PriceInfo { get; private set; }
    public Quantity Quantity { get; }
    public VatRate Vat { get; }
    public DeliveryOrder? Order { get; }
    public DateTimeOffset CreatedOn { get; private set; }
    public IEnumerable<DeliveryBatch> Batches { get; } = new List<DeliveryBatch>();
}