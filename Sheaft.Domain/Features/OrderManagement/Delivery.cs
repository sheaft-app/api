using System.Collections.ObjectModel;

namespace Sheaft.Domain.OrderManagement;

public class Delivery : AggregateRoot
{
    private Delivery(){}
    
    public Delivery(DeliveryDate date, DeliveryAddress address, IEnumerable<OrderId> orderIdentifiers, SupplierId supplierIdentifier)
    {
        Identifier = DeliveryId.New();
        ScheduledAt = date;
        Address = address;
        Status = DeliveryStatus.Pending;
        SupplierIdentifier = supplierIdentifier;

        SetOrders(orderIdentifiers);
    }

    public DeliveryId Identifier { get; }
    public DeliveryCode? Code { get; private set; }
    public DeliveryStatus Status { get; private set; }
    public DeliveryDate ScheduledAt { get; private set; }
    public DateTimeOffset? DeliveredOn { get; private set; }
    public DeliveryAddress Address { get; private set; }
    public SupplierId SupplierIdentifier { get; private set; }
    public ReadOnlyCollection<DeliveryOrder> Orders { get; private set; }
    
    public Result SetOrders(IEnumerable<OrderId> orderIdentifiers)
    {
        Orders = new ReadOnlyCollection<DeliveryOrder>(orderIdentifiers.Select(o => new DeliveryOrder(o)).ToList());
        return Result.Success();
    }

    internal Result Schedule(DeliveryCode code, DeliveryDate scheduledOn, DateTimeOffset? currentDateTime = null)
    {
        if (Status != DeliveryStatus.Pending)
            return Result.Failure(ErrorKind.BadRequest, "delivery.deliver.requires.pending.delivery");
        
        Code = code;
        return Reschedule(scheduledOn, currentDateTime);
    }

    internal Result Reschedule(DeliveryDate newDeliveryDate, DateTimeOffset? currentDateTime = null)
    {
        if (newDeliveryDate.Value < (currentDateTime ?? DateTimeOffset.UtcNow))
            return Result.Failure(ErrorKind.BadRequest, "delivery.confirm.requires.incoming.deliverydate");
        
        ScheduledAt = newDeliveryDate;
        Status = DeliveryStatus.Scheduled;
        return Result.Success();
    }

    internal Result Deliver(DateTimeOffset? currentDateTime = null)
    {
        if (Status != DeliveryStatus.Scheduled)
            return Result.Failure(ErrorKind.BadRequest, "delivery.deliver.requires.scheduled.delivery");
        
        DeliveredOn = currentDateTime ?? DateTimeOffset.UtcNow;
        Status = DeliveryStatus.Delivered;
        return Result.Success();
    }
}

public record DeliveryOrder(OrderId OrderIdentifier);