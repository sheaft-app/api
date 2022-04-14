namespace Sheaft.Domain.OrderManagement;

public class Order : AggregateRoot
{
    private Order(){}
    
    private Order(OrderStatus status, SupplierId supplierIdentifier, CustomerId customerIdentifier, IEnumerable<OrderLine>? lines = null, OrderReference? reference = null, string? externalCode = null)
    {
        Identifier = OrderId.New();
        Reference = reference;
        ExternalCode = externalCode;
        Status = status;
        SupplierIdentifier = supplierIdentifier;
        CustomerIdentifier = customerIdentifier;
        
        SetLines(lines);
    }

    internal static Order CreateDraft(SupplierId supplierIdentifier, CustomerId customerIdentifier)
    {
        return new Order(OrderStatus.Draft, supplierIdentifier, customerIdentifier);
    }

    public static Order Create(OrderReference reference, SupplierId supplierIdentifier, CustomerId customerIdentifier, IEnumerable<OrderLine> lines, string? externalCode = null)
    {
        return new Order(OrderStatus.Pending, supplierIdentifier, customerIdentifier, lines, reference, externalCode);
    }

    public OrderId Identifier { get; }
    public OrderReference? Reference { get; private set; }
    public TotalWholeSalePrice TotalWholeSalePrice { get; private set; }
    public TotalVatPrice TotalVatPrice { get; private set; }
    public TotalOnSalePrice TotalOnSalePrice { get; private set; }
    public string? ExternalCode { get; private set; }
    public OrderStatus Status { get; private set; }
    public DateTimeOffset? FulfilledOn { get; private set; }
    public DateTimeOffset? AcceptedOn { get; private set; }
    public DateTimeOffset? CompletedOn { get; private set; }
    public int ProductsCount { get; private set; }
    public string? FailureReason { get; private set; }
    public CustomerId CustomerIdentifier { get; private set; }
    public SupplierId SupplierIdentifier { get; private set; }
    public IEnumerable<OrderLine> Lines { get; private set; } = new List<OrderLine>();
    
    internal Result Publish(OrderReference reference, IEnumerable<OrderLine> lines)
    {
        if (Status != OrderStatus.Draft)
            return Result.Failure(ErrorKind.BadRequest, "order.publish.requires.draft");
        
        if (lines == null || !lines.Any())
            return Result.Failure(ErrorKind.BadRequest, "order.publish.requires.lines");
        
        SetLines(lines);
        
        Reference = reference;
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

    public Result Accept(DateTimeOffset? currentDateTime = null)
    {
        if (Status != OrderStatus.Pending)
            return Result.Failure(ErrorKind.BadRequest, "order.accept.requires.pending.status");
        
        AcceptedOn = currentDateTime ?? DateTimeOffset.UtcNow;
        Status = OrderStatus.Accepted;
        return Result.Success();
    }

    internal Result Fulfill(DateTimeOffset? currentDateTime = null)
    {
        if (Status != OrderStatus.Accepted)
            return Result.Failure(ErrorKind.BadRequest, "order.fulfill.requires.accepted.status");
        
        FulfilledOn = currentDateTime ?? DateTimeOffset.UtcNow;
        Status = OrderStatus.Fulfilled;
        return Result.Success();
    }

    public Result Refuse(string? refusalReason, DateTimeOffset? currentDateTime = null)
    {
        if (Status != OrderStatus.Pending)
            return Result.Failure(ErrorKind.BadRequest, "order.refuse.requires.pending.status");
        
        Status = OrderStatus.Refused;
        CompletedOn = currentDateTime ?? DateTimeOffset.UtcNow;
        FailureReason = refusalReason;
        return Result.Success();
    }

    public Result Cancel(string? cancelReason, DateTimeOffset? currentDateTime)
    {
        if (Status != OrderStatus.Accepted && Status != OrderStatus.Fulfilled)
            return Result.Failure(ErrorKind.BadRequest, "order.cancel.requires.accepted.or.fulfilled.status");
        
        Status = OrderStatus.Cancelled;
        CompletedOn = currentDateTime ?? DateTimeOffset.UtcNow;
        FailureReason = cancelReason;
        return Result.Success();
    }

    internal Result Complete(DateTimeOffset? currentDateTime)
    {
        if (Status != OrderStatus.Fulfilled)
            return Result.Failure(ErrorKind.BadRequest, "order.deliver.requires.fulfilled.status");
        
        Status = OrderStatus.Completed;
        CompletedOn = currentDateTime ?? DateTimeOffset.UtcNow;
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