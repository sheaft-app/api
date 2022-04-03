using System.Collections.ObjectModel;

namespace Sheaft.Domain.OrderManagement;

public class Order : AggregateRoot
{
    private Order(){}
    
    private Order(OrderStatus status, SupplierId supplierIdentifier, CustomerId customerIdentifier, DeliveryAddress deliveryAddress, BillingAddress billingAddress, IEnumerable<OrderLine>? lines = null, OrderDeliveryDate? deliveryDate = null, OrderCode? code = null, string? externalCode = null)
    {
        Identifier = OrderId.New();
        Code = code;
        ExternalCode = externalCode;
        Status = status;
        SupplierIdentifier = supplierIdentifier;
        CustomerIdentifier = customerIdentifier;
        BillingAddress = billingAddress;
        DeliveryAddress = deliveryAddress;
        DeliveryDate = deliveryDate;
        
        SetLines(lines);
    }

    public static Order CreateDraft(SupplierId supplierIdentifier, CustomerId customerIdentifier,
        DeliveryAddress deliveryAddress, BillingAddress billingAddress)
    {
        return new Order(OrderStatus.Draft, supplierIdentifier, customerIdentifier, deliveryAddress, billingAddress);
    }

    public static Order Create(OrderCode code, OrderDeliveryDate deliveryDate, SupplierId supplierIdentifier, CustomerId customerIdentifier,
        DeliveryAddress deliveryAddress, BillingAddress billingAddress, IEnumerable<OrderLine> lines, string? externalCode = null)
    {
        return new Order(OrderStatus.Pending, supplierIdentifier, customerIdentifier, deliveryAddress, billingAddress, lines, deliveryDate, code, externalCode);
    }

    public OrderId Identifier { get; }
    public OrderCode? Code { get; private set; }
    public string? ExternalCode { get; private set; }
    public OrderStatus Status { get; private set; }
    public OrderDeliveryDate? DeliveryDate { get; private set; }
    public CustomerId CustomerIdentifier { get; private set; }
    public SupplierId SupplierIdentifier { get; private set; }
    public DeliveryAddress DeliveryAddress { get; private set; }
    public BillingAddress BillingAddress { get; private set; }
    public Price TotalPrice { get; private set; }
    public int ProductsCount { get; private set; }

    public string? FailureReason { get; private set; }
    public ReadOnlyCollection<OrderLine> Lines { get; private set; }
    
    internal Result Publish(OrderCode code, OrderDeliveryDate deliveryDate, IEnumerable<OrderLine>? lines = null)
    {
        if (Status != OrderStatus.Draft)
            return Result.Failure(ErrorKind.BadRequest, "order.publish.requires.draft");
        
        if (lines != null && !lines.Any() || !Lines.Any())
            return Result.Failure(ErrorKind.BadRequest, "order.publish.requires.lines");
        
        Code = code;
        DeliveryDate = deliveryDate;

        if(lines != null)
            SetLines(lines);
        
        Status = OrderStatus.Pending;
        return Result.Success();
    }
    
    public Result UpdateDraftLines(IEnumerable<OrderLine> lines)
    {
        if (Status != OrderStatus.Draft)
            return Result.Failure(ErrorKind.BadRequest, "order.update.lines.requires.draft");
        
        SetLines(lines);
        return Result.Success();
    }

    public Result Accept(Maybe<OrderDeliveryDate> newOrderDeliveryDate)
    {
        if (Status != OrderStatus.Pending)
            return Result.Failure(ErrorKind.BadRequest, "order.accept.requires.pending.status");
        
        if (newOrderDeliveryDate.HasValue)
            DeliveryDate = newOrderDeliveryDate.Value;

        Status = OrderStatus.Accepted;
        return Result.Success();
    }

    public Result Complete(Maybe<OrderDeliveryDate> newDeliveryDate)
    {
        if (Status != OrderStatus.Accepted)
            return Result.Failure(ErrorKind.BadRequest, "order.complete.requires.accepted.status");
        
        if (newDeliveryDate.HasValue)
            DeliveryDate = newDeliveryDate.Value;

        Status = OrderStatus.Ready;
        return Result.Success();
    }

    public Result Refuse(string? refusalReason)
    {
        if (Status != OrderStatus.Pending)
            return Result.Failure(ErrorKind.BadRequest, "order.refuse.requires.pending.status");
        
        Status = OrderStatus.Refused;
        FailureReason = refusalReason;
        return Result.Success();
    }

    public Result Cancel(string? cancelReason)
    {
        if (Status != OrderStatus.Accepted && Status != OrderStatus.Ready)
            return Result.Failure(ErrorKind.BadRequest, "order.cancel.requires.accepted.or.ready.status");
        
        Status = OrderStatus.Cancelled;
        FailureReason = cancelReason;
        return Result.Success();
    }

    private void SetLines(IEnumerable<OrderLine>? lines)
    {
        var orderLines = lines?.ToList() ?? new List<OrderLine>();
        Lines = new ReadOnlyCollection<OrderLine>(orderLines);
        TotalPrice = GetTotalPrice();
        ProductsCount = GetProductsCount();
    }
    
    private int GetProductsCount()
    {
        return Lines.Sum(l => l.Quantity.Value);
    }

    private Price GetTotalPrice()
    {
        return new Price(Lines.Sum(l => l.TotalPrice.Value));
    }
}