namespace Sheaft.Domain.OrderManagement;

public class Order : AggregateRoot
{
    private Order(){}
    
    private Order(OrderStatus status, SupplierId supplierId, CustomerId customerId, IEnumerable<OrderLine>? lines = null, OrderReference? reference = null)
    {
        Id = OrderId.New();
        Reference = reference;
        Status = status;
        SupplierId = supplierId;
        CustomerId = customerId;
        CreatedOn = DateTimeOffset.UtcNow;
        UpdatedOn = DateTimeOffset.UtcNow;
        
        SetLines(lines);
    }

    internal static Order CreateDraft(SupplierId supplierIdentifier, CustomerId customerIdentifier)
    {
        return new Order(OrderStatus.Draft, supplierIdentifier, customerIdentifier);
    }

    public OrderId Id { get; }
    public OrderReference? Reference { get; private set; }
    public TotalWholeSalePrice TotalWholeSalePrice { get; private set; }
    public TotalVatPrice TotalVatPrice { get; private set; }
    public TotalOnSalePrice TotalOnSalePrice { get; private set; }
    public OrderStatus Status { get; private set; }
    public DateTimeOffset? PublishedOn { get; private set; }
    public DateTimeOffset? FulfilledOn { get; private set; }
    public DateTimeOffset? AcceptedOn { get; private set; }
    public DateTimeOffset? CompletedOn { get; private set; }
    public int ProductsCount { get; private set; }
    public string? FailureReason { get; private set; }
    public CustomerId CustomerId { get; private set; }
    public SupplierId SupplierId { get; private set; }
    public IEnumerable<OrderLine> Lines { get; private set; } = new List<OrderLine>();
    public InvoiceId? InvoiceId { get; set; }
    public DeliveryId? DeliveryId { get; set; }
    public DateTimeOffset CreatedOn { get; private set; }
    public DateTimeOffset UpdatedOn { get; private set; }

    internal Result Publish(OrderReference reference, IEnumerable<OrderLine> lines, DateTimeOffset? currentDateTime = null)
    {
        if (Status != OrderStatus.Draft)
            return Result.Failure(ErrorKind.BadRequest, "order.publish.requires.draft");
        
        if (lines == null || !lines.Any())
            return Result.Failure(ErrorKind.BadRequest, "order.publish.requires.lines");
        
        SetLines(lines);
        
        Reference = reference;
        Status = OrderStatus.Pending;
        PublishedOn = currentDateTime ?? DateTimeOffset.UtcNow;
        UpdatedOn = currentDateTime ?? DateTimeOffset.UtcNow;
        return Result.Success();
    }

    public void AssignDelivery(DeliveryId deliveryIdentifier)
    {
        DeliveryId = deliveryIdentifier;
    }

    public void AttachInvoice(InvoiceId invoiceIdentifier)
    {
        InvoiceId = invoiceIdentifier;
    }

    public void DetachInvoice()
    {
        InvoiceId = null;
    }
    
    public Result UpdateDraftLines(IEnumerable<OrderLine> lines)
    {
        if (Status != OrderStatus.Draft)
            return Result.Failure(ErrorKind.BadRequest, "order.update.lines.requires.draft");
        
        SetLines(lines);
        UpdatedOn = DateTimeOffset.UtcNow;
        return Result.Success();
    }

    public Result Accept(DateTimeOffset? currentDateTime = null)
    {
        if (Status != OrderStatus.Pending)
            return Result.Failure(ErrorKind.BadRequest, "order.accept.requires.pending.status");
        
        AcceptedOn = currentDateTime ?? DateTimeOffset.UtcNow;
        UpdatedOn = currentDateTime ?? DateTimeOffset.UtcNow;
        Status = OrderStatus.Accepted;
        return Result.Success();
    }

    internal Result Fulfill(DateTimeOffset? currentDateTime = null)
    {
        if (Status != OrderStatus.Accepted)
            return Result.Failure(ErrorKind.BadRequest, "order.fulfill.requires.accepted.status");
        
        FulfilledOn = currentDateTime ?? DateTimeOffset.UtcNow;
        UpdatedOn = currentDateTime ?? DateTimeOffset.UtcNow;
        Status = OrderStatus.Fulfilled;
        return Result.Success();
    }

    public Result Refuse(string? refusalReason, DateTimeOffset? currentDateTime = null)
    {
        if (Status != OrderStatus.Pending)
            return Result.Failure(ErrorKind.BadRequest, "order.refuse.requires.pending.status");
        
        Status = OrderStatus.Refused;
        CompletedOn = currentDateTime ?? DateTimeOffset.UtcNow;
        UpdatedOn = currentDateTime ?? DateTimeOffset.UtcNow;
        FailureReason = refusalReason;
        return Result.Success();
    }

    public Result Cancel(string? cancelReason, DateTimeOffset? currentDateTime)
    {
        if (Status != OrderStatus.Accepted && Status != OrderStatus.Fulfilled)
            return Result.Failure(ErrorKind.BadRequest, "order.cancel.requires.accepted.or.fulfilled.status");
        
        Status = OrderStatus.Cancelled;
        CompletedOn = currentDateTime ?? DateTimeOffset.UtcNow;
        UpdatedOn = currentDateTime ?? DateTimeOffset.UtcNow;
        FailureReason = cancelReason;
        return Result.Success();
    }

    internal Result Complete(DateTimeOffset? currentDateTime)
    {
        if (Status != OrderStatus.Fulfilled)
            return Result.Failure(ErrorKind.BadRequest, "order.deliver.requires.fulfilled.status");
        
        Status = OrderStatus.Completed;
        CompletedOn = currentDateTime ?? DateTimeOffset.UtcNow;
        UpdatedOn = currentDateTime ?? DateTimeOffset.UtcNow;
        return Result.Success();
    }

    private void SetLines(IEnumerable<OrderLine>? lines)
    {
        var orderLines = lines?.ToList() ?? new List<OrderLine>();
        Lines = new List<OrderLine>(orderLines);
        
        ProductsCount = GetProductsCount();
        CalculatePrices();
    }

    private void CalculatePrices()
    {
        TotalWholeSalePrice = GetTotalWholeSalePrice();
        TotalVatPrice = GetTotalVatPrice();
        TotalOnSalePrice = GetTotalOnSalePrice();
    }

    private int GetProductsCount()
    {
        return Lines.Sum(l => l.Quantity.Value);
    }

    private TotalWholeSalePrice GetTotalWholeSalePrice()
    {
        return new TotalWholeSalePrice(Lines.Sum(l => l.PriceInfo.WholeSalePrice.Value));
    }

    private TotalOnSalePrice GetTotalOnSalePrice()
    {
        return new TotalOnSalePrice(Lines.Sum(l => l.PriceInfo.OnSalePrice.Value));
    }

    private TotalVatPrice GetTotalVatPrice()
    {
        return new TotalVatPrice(Lines.Sum(l => l.PriceInfo.VatPrice.Value));
    }
}