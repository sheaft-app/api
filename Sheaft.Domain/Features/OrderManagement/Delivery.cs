namespace Sheaft.Domain.OrderManagement;

public class Delivery : AggregateRoot
{
    private Delivery()
    {
    }

    public Delivery(DeliveryDate date, DeliveryAddress address, SupplierId supplierId, CustomerId customerId, IEnumerable<Order> orders)
    {
        Id = DeliveryId.New();
        ScheduledAt = date;
        Address = address;
        Status = DeliveryStatus.Pending;
        SupplierId = supplierId;
        CustomerId = customerId;
        CreatedOn = DateTimeOffset.UtcNow;
        UpdatedOn = DateTimeOffset.UtcNow;
        
        var result = AssignOrders(orders);
        if (result.IsFailure)
            throw new InvalidOperationException(result.Error.Code);
        
        CalculatePrices();
    }

    public DeliveryId Id { get; }
    public DeliveryReference? Reference { get; private set; }
    public DeliveryStatus Status { get; private set; }
    public DeliveryDate ScheduledAt { get; private set; }
    public DateTimeOffset? DeliveredOn { get; private set; }
    public TotalWholeSalePrice TotalWholeSalePrice { get; private set; }
    public TotalVatPrice TotalVatPrice { get; private set; }
    public TotalOnSalePrice TotalOnSalePrice { get; private set; }
    public DeliveryAddress Address { get; }
    public string? Comments { get; private set; }
    public SupplierId SupplierId { get; }
    public CustomerId CustomerId { get; }
    public DateTimeOffset CreatedOn { get; private set; }
    public DateTimeOffset UpdatedOn { get; private set; }
    public IEnumerable<DeliveryLine> Lines { get; private set; } = new List<DeliveryLine>();
    public IEnumerable<DeliveryLine> Adjustments { get; private set; } = new List<DeliveryLine>();

    private Result AssignOrders(IEnumerable<Order> orders)
    {
        if(orders == null || !orders.Any())
            return Result.Failure(ErrorKind.BadRequest, "delivery.requires.orders");
        
        if (orders.Any(o => o.Status == OrderStatus.Draft))
            return Result.Failure(ErrorKind.BadRequest, "delivery.requires.published.orders");
        
        if (orders.GroupBy(o => o.CustomerId).Count() > 1)
            return Result.Failure(ErrorKind.BadRequest, "delivery.orders.must.have.same.customer");
        
        if (orders.First().CustomerId != CustomerId)
            return Result.Failure(ErrorKind.BadRequest, "delivery.orders.must.be.from.customer");
        
        if (orders.GroupBy(o => o.SupplierId).Count() > 1)
            return Result.Failure(ErrorKind.BadRequest, "delivery.orders.must.have.same.supplier");
        
        if (orders.First().SupplierId != SupplierId)
            return Result.Failure(ErrorKind.BadRequest, "delivery.orders.must.be.from.supplier");

        foreach (var order in orders)
            order.AssignDelivery(Id);
        
        return Result.Success();
    }

    internal Result Schedule(DeliveryReference reference, DeliveryDate scheduledOn, DateTimeOffset? currentDateTime = null)
    {
        var result = CanScheduleDelivery(scheduledOn, currentDateTime);
        if (result.IsFailure) 
            return result;

        Reference = reference;
        Status = DeliveryStatus.Scheduled;
        ScheduledAt = scheduledOn;
        UpdatedOn = DateTimeOffset.UtcNow;
        
        return Result.Success();
    }

    public void UpdateLines(IEnumerable<DeliveryLine> lines)
    {
        Lines = lines.ToList();
        CalculatePrices();
        UpdatedOn = DateTimeOffset.UtcNow;
    }

    internal Result CanScheduleDelivery(DeliveryDate scheduledOn, DateTimeOffset? currentDateTime)
    {
        if (Status != DeliveryStatus.Pending)
            return Result.Failure(ErrorKind.BadRequest, "delivery.deliver.requires.pending.delivery");

        if (!Lines.Any())
            return Result.Failure(ErrorKind.BadRequest, "delivery.requires.lines");

        if (scheduledOn.Value < (currentDateTime ?? DateTimeOffset.UtcNow))
            return Result.Failure(ErrorKind.BadRequest, "delivery.confirm.requires.incoming.deliverydate");

        return Result.Success();
    }

    internal Result ChangeDeliveryDate(DeliveryDate scheduledOn, DateTimeOffset? currentDateTime)
    {
        if (Status != DeliveryStatus.Pending)
            return Result.Failure(ErrorKind.BadRequest, "delivery.deliver.requires.pending.delivery");

        if (scheduledOn.Value < (currentDateTime ?? DateTimeOffset.UtcNow))
            return Result.Failure(ErrorKind.BadRequest, "delivery.confirm.requires.incoming.deliverydate");

        ScheduledAt = scheduledOn;
        UpdatedOn = DateTimeOffset.UtcNow;
        return Result.Success();
    }

    internal Result Deliver(IEnumerable<DeliveryLine> adjustments, string? comments = null, DateTimeOffset? currentDateTime = null)
    {
        if (Status != DeliveryStatus.Scheduled)
            return Result.Failure(ErrorKind.BadRequest, "delivery.deliver.requires.scheduled.delivery");

        Adjustments = adjustments.ToList();
        DeliveredOn = currentDateTime ?? DateTimeOffset.UtcNow;
        UpdatedOn = DateTimeOffset.UtcNow;
        Status = DeliveryStatus.Delivered;
        Comments = comments;
        
        CalculatePrices();

        return Result.Success();
    }

    private void CalculatePrices()
    {
        TotalWholeSalePrice = GetTotalWholeSalePrice();
        TotalVatPrice = GetTotalVatPrice();
        TotalOnSalePrice = GetTotalOnSalePrice();
    }

    private TotalWholeSalePrice GetTotalWholeSalePrice()
    {
        return new TotalWholeSalePrice(Lines.Sum(l => l.PriceInfo.WholeSalePrice.Value) + Adjustments.Sum(l => l.PriceInfo.WholeSalePrice.Value));
    }

    private TotalOnSalePrice GetTotalOnSalePrice()
    {
        return new TotalOnSalePrice(Lines.Sum(l => l.PriceInfo.OnSalePrice.Value) + Adjustments.Sum(l => l.PriceInfo.OnSalePrice.Value));
    }

    private TotalVatPrice GetTotalVatPrice()
    {
        return new TotalVatPrice(Lines.Sum(l => l.PriceInfo.VatPrice.Value) + Adjustments.Sum(l => l.PriceInfo.VatPrice.Value));
    }
}