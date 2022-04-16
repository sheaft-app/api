namespace Sheaft.Domain.OrderManagement;

public class Delivery : AggregateRoot
{
    private Delivery()
    {
    }

    public Delivery(DeliveryDate date, DeliveryAddress address, SupplierId supplierIdentifier, CustomerId customerIdentifier, IEnumerable<Order> orders)
    {
        Identifier = DeliveryId.New();
        ScheduledAt = date;
        Address = address;
        Status = DeliveryStatus.Pending;
        SupplierIdentifier = supplierIdentifier;
        CustomerIdentifier = customerIdentifier;
        
        var result = AssignOrders(orders);
        if (result.IsFailure)
            throw new InvalidOperationException(result.Error.Code);
        
        CalculatePrices();
    }

    public DeliveryId Identifier { get; }
    public DeliveryReference? Reference { get; private set; }
    public DeliveryStatus Status { get; private set; }
    public DeliveryDate ScheduledAt { get; private set; }
    public DateTimeOffset? DeliveredOn { get; private set; }
    public TotalWholeSalePrice TotalWholeSalePrice { get; private set; }
    public TotalVatPrice TotalVatPrice { get; private set; }
    public TotalOnSalePrice TotalOnSalePrice { get; private set; }
    public DeliveryAddress Address { get; }
    public SupplierId SupplierIdentifier { get; }
    public CustomerId CustomerIdentifier { get; }
    public IEnumerable<DeliveryLine> Lines { get; private set; } = new List<DeliveryLine>();
    public IEnumerable<DeliveryLine> Adjustments { get; private set; } = new List<DeliveryLine>();

    private Result AssignOrders(IEnumerable<Order> orders)
    {
        if(orders == null || !orders.Any())
            return Result.Failure(ErrorKind.BadRequest, "delivery.requires.orders");
        
        if (orders.Any(o => o.Status == OrderStatus.Draft))
            return Result.Failure(ErrorKind.BadRequest, "delivery.requires.published.orders");
        
        if (orders.GroupBy(o => o.CustomerIdentifier).Count() > 1)
            return Result.Failure(ErrorKind.BadRequest, "delivery.orders.must.have.same.customer");
        
        if (orders.First().CustomerIdentifier != CustomerIdentifier)
            return Result.Failure(ErrorKind.BadRequest, "delivery.orders.must.be.from.customer");
        
        if (orders.GroupBy(o => o.SupplierIdentifier).Count() > 1)
            return Result.Failure(ErrorKind.BadRequest, "delivery.orders.must.have.same.supplier");
        
        if (orders.First().SupplierIdentifier != SupplierIdentifier)
            return Result.Failure(ErrorKind.BadRequest, "delivery.orders.must.be.from.supplier");

        foreach (var order in orders)
            order.AssignDelivery(Identifier);
        
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
        
        return Result.Success();
    }

    public Result UpdateLines(IEnumerable<DeliveryLine> lines)
    {
        Lines = lines.ToList();
        CalculatePrices();
        return Result.Success();
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
        return Result.Success();
    }

    internal Result Deliver(IEnumerable<DeliveryLine> adjustments, DateTimeOffset? currentDateTime = null)
    {
        if (Status != DeliveryStatus.Scheduled)
            return Result.Failure(ErrorKind.BadRequest, "delivery.deliver.requires.scheduled.delivery");

        Adjustments = adjustments.ToList();
        DeliveredOn = currentDateTime ?? DateTimeOffset.UtcNow;
        Status = DeliveryStatus.Delivered;
        
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