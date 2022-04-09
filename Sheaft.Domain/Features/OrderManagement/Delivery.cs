namespace Sheaft.Domain.OrderManagement;

public class Delivery : AggregateRoot
{
    private Delivery()
    {
    }

    public Delivery(DeliveryDate date, DeliveryAddress address, SupplierId supplierIdentifier, IEnumerable<Order> orders)
    {
        Identifier = DeliveryId.New();
        ScheduledAt = date;
        Address = address;
        Status = DeliveryStatus.Pending;
        SupplierIdentifier = supplierIdentifier;

        var result = SetOrders(orders);
        if (result.IsFailure)
            throw new InvalidOperationException(result.Error.Code);
    }

    public DeliveryId Identifier { get; }
    public DeliveryReference? Reference { get; private set; }
    public DeliveryStatus Status { get; private set; }
    public DeliveryDate ScheduledAt { get; private set; }
    public DateTimeOffset? DeliveredOn { get; private set; }
    public DeliveryAddress Address { get; }
    public SupplierId SupplierIdentifier { get; }
    public IEnumerable<DeliveryOrder> Orders { get; private set; }
    public IEnumerable<DeliveryLine> Lines { get; private set; }
    public IEnumerable<DeliveryLine> Adjustments { get; private set; }
    public IEnumerable<DeliveryBatch> Batches { get; private set; }

    private Result SetOrders(IEnumerable<Order> orders)
    {
        if (orders.GroupBy(o => o.CustomerIdentifier).Count() > 1)
            return Result.Failure(ErrorKind.BadRequest, "delivery.orders.must.have.same.customer");
        
        if (orders.GroupBy(o => o.SupplierIdentifier).Count() > 1)
            return Result.Failure(ErrorKind.BadRequest, "delivery.orders.must.have.same.supplier");
        
        Orders = orders.Select(o => new DeliveryOrder(o.Identifier)).ToList();
        return Result.Success();
    }

    internal Result Schedule(DeliveryReference reference, DeliveryDate scheduledOn, IEnumerable<DeliveryLine> lines, IEnumerable<DeliveryBatch>? batches, DateTimeOffset? currentDateTime = null)
    {
        var result = CanScheduleDelivery(scheduledOn, lines, currentDateTime);
        if (result.IsFailure) 
            return result;

        Lines = lines.ToList();
        Batches = batches?.ToList() ?? new List<DeliveryBatch>();
        Reference = reference;
        Status = DeliveryStatus.Scheduled;
        
        ScheduledAt = scheduledOn;
        return Result.Success();
    }

    internal Result CanScheduleDelivery(DeliveryDate scheduledOn, IEnumerable<DeliveryLine> lines, DateTimeOffset? currentDateTime)
    {
        if (Status != DeliveryStatus.Pending)
            return Result.Failure(ErrorKind.BadRequest, "delivery.deliver.requires.pending.delivery");

        if (!lines.Any())
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

    internal Result Deliver(IEnumerable<DeliveryLine> linesAdjustments, DateTimeOffset? currentDateTime = null)
    {
        if (Status != DeliveryStatus.Scheduled)
            return Result.Failure(ErrorKind.BadRequest, "delivery.deliver.requires.scheduled.delivery");

        Adjustments = linesAdjustments.ToList();
        DeliveredOn = currentDateTime ?? DateTimeOffset.UtcNow;
        Status = DeliveryStatus.Delivered;
        return Result.Success();
    }
}