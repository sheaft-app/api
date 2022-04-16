using Sheaft.Domain.OrderManagement;

namespace Sheaft.Domain.BillingManagement;

public record InvoiceLine
{
    private InvoiceLine()
    {
    }

    protected InvoiceLine(bool editable, string identifier, string name, Quantity quantity, UnitPrice unitPrice,
        VatRate vat, InvoiceDelivery delivery, DeliveryOrder order)
    {
        Identifier = identifier;
        Name = name;
        Quantity = quantity;
        Vat = vat;
        PriceInfo = new LinePrice(unitPrice, vat, quantity);
        IsEditable = editable;
        Order = order;
        Delivery = delivery;
    }

    public static InvoiceLine CreateLineForDeliveryOrder(string identifier, string name, Quantity quantity, UnitPrice unitPrice, VatRate vat, InvoiceDelivery delivery, DeliveryOrder order)
    {
        return new InvoiceLine(true, identifier, name, quantity, unitPrice, vat, delivery, order);
    }

    public string Identifier { get; private set; }
    public string Name { get; private set; }
    public LinePrice PriceInfo { get; private set; }
    public Quantity Quantity { get; }
    public VatRate Vat { get; }
    public bool IsEditable { get; }
    public DeliveryOrder Order { get; }
    public InvoiceDelivery Delivery { get; }
}