using System.Collections.ObjectModel;
using Sheaft.Domain.AgreementManagement;

namespace Sheaft.Domain.OrderManagement;

public class Order : AggregateRoot
{
    private Order(){}
    
    private Order(OrderStatus status, SupplierId supplierIdentifier, CustomerId customerIdentifier, IEnumerable<OrderLine>? lines = null, OrderCode? code = null, string? externalCode = null)
    {
        Identifier = OrderId.New();
        Code = code;
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

    public static Order Create(OrderCode code, SupplierId supplierIdentifier, CustomerId customerIdentifier, IEnumerable<OrderLine> lines, string? externalCode = null)
    {
        return new Order(OrderStatus.Pending, supplierIdentifier, customerIdentifier, lines, code, externalCode);
    }

    public OrderId Identifier { get; }
    public OrderCode? Code { get; private set; }
    public string? ExternalCode { get; private set; }
    public OrderStatus Status { get; private set; }
    public UnitPrice TotalPrice { get; private set; }
    public DateTimeOffset? FulfilledOn { get; private set; }
    public DateTimeOffset? AcceptedOn { get; private set; }
    public DateTimeOffset? CompletedOn { get; private set; }
    public int ProductsCount { get; private set; }
    public string? FailureReason { get; private set; }
    public CustomerId CustomerIdentifier { get; private set; }
    public SupplierId SupplierIdentifier { get; private set; }
    public IEnumerable<OrderLine> Lines { get; private set; } = new List<OrderLine>();
    
    internal Result Publish(OrderCode code, IEnumerable<OrderLine>? lines = null)
    {
        if (Status != OrderStatus.Draft)
            return Result.Failure(ErrorKind.BadRequest, "order.publish.requires.draft");
        
        if (lines != null && !lines.Any() || !Lines.Any())
            return Result.Failure(ErrorKind.BadRequest, "order.publish.requires.lines");
        
        Code = code;

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
            return Result.Failure(ErrorKind.BadRequest, "order.cancel.requires.accepted.or.ready.status");
        
        Status = OrderStatus.Cancelled;
        CompletedOn = currentDateTime ?? DateTimeOffset.UtcNow;
        FailureReason = cancelReason;
        return Result.Success();
    }

    internal Result MarkAsCompleted()
    {
        if (Status != OrderStatus.Fulfilled)
            return Result.Failure(ErrorKind.BadRequest, "order.deliver.requires.fulfilled.status");
        
        Status = OrderStatus.Completed;
        return Result.Success();
    }

    private void SetLines(IEnumerable<OrderLine>? lines)
    {
        var orderLines = lines?.ToList() ?? new List<OrderLine>();
        Lines = new List<OrderLine>(orderLines);
        TotalPrice = GetTotalPrice();
        ProductsCount = GetProductsCount();
    }
    
    private int GetProductsCount()
    {
        return Lines.Sum(l => l.Quantity.Value);
    }

    private UnitPrice GetTotalPrice()
    {
        return new UnitPrice(Lines.Sum(l => l.TotalPrice.Value));
    }
}