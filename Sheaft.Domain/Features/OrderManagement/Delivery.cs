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
    public DeliveryAddress Address { get; private set; }
    public SupplierId SupplierIdentifier { get; private set; }
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

        var lines = orders
            .SelectMany(o => o.Lines.Select(l => l))
            .GroupBy(o => o.Identifier)
            .Select(gr =>
            {
                var orderLine = gr.First();
                return new DeliveryLine(orderLine.Identifier,
                    orderLine.LineKind == OrderLineKind.Product
                        ? DeliveryLineKind.Product
                        : DeliveryLineKind.Returnable, orderLine.Reference, orderLine.Reference,
                    new OrderedQuantity(gr.Sum(g => g.Quantity.Value)), orderLine.UnitPrice, orderLine.Vat);
            }).ToList();

        Lines = lines;
        return Result.Success();
    }

    internal Result Schedule(DeliveryReference reference, DeliveryDate scheduledOn, IEnumerable<DeliveryBatch>? batches, DateTimeOffset? currentDateTime = null)
    {
        if (Status != DeliveryStatus.Pending)
            return Result.Failure(ErrorKind.BadRequest, "delivery.deliver.requires.pending.delivery");

        Batches = batches ?? new List<DeliveryBatch>();
        Reference = reference;
        Status = DeliveryStatus.Scheduled;
        return Reschedule(scheduledOn, currentDateTime);
    }

    internal Result Reschedule(DeliveryDate newDeliveryDate, DateTimeOffset? currentDateTime = null)
    {
        if (newDeliveryDate.Value < (currentDateTime ?? DateTimeOffset.UtcNow))
            return Result.Failure(ErrorKind.BadRequest, "delivery.confirm.requires.incoming.deliverydate");

        ScheduledAt = newDeliveryDate;
        return Result.Success();
    }

    internal Result Deliver(IEnumerable<DeliveryLine> linesAdjustments, DateTimeOffset? currentDateTime = null)
    {
        if (Status != DeliveryStatus.Scheduled)
            return Result.Failure(ErrorKind.BadRequest, "delivery.deliver.requires.scheduled.delivery");

        Adjustments = linesAdjustments;
        DeliveredOn = currentDateTime ?? DateTimeOffset.UtcNow;
        Status = DeliveryStatus.Delivered;
        return Result.Success();
    }
}