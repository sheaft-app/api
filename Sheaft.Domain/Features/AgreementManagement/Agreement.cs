using System.Collections.ObjectModel;

namespace Sheaft.Domain.AgreementManagement;

public class Agreement : AggregateRoot
{
    private Agreement(){}
    
    private Agreement(AgreementOwner owner, SupplierId supplierId, CustomerId customerId, CatalogId catalogId)
    {
        Id = AgreementId.New();
        Owner = owner;
        SupplierId = supplierId;
        CustomerId = customerId;
        CatalogId = catalogId;
        Status = AgreementStatus.Pending;
        CreatedOn = DateTimeOffset.UtcNow;
        UpdatedOn = DateTimeOffset.UtcNow;
    }

    public static Agreement CreateAndSendAgreementToSupplier(SupplierId supplierIdentifier, CustomerId customerIdentifier,
        CatalogId catalogIdentifier)
    {
        return new Agreement(AgreementOwner.Customer, supplierIdentifier, customerIdentifier, catalogIdentifier);
    }

    public static Agreement CreateAndSendAgreementToCustomer(SupplierId supplierIdentifier, CustomerId customerIdentifier,
        CatalogId catalogIdentifier, List<DeliveryDay> deliveryDays, int? orderDelayInHoursBeforeDeliveryDay = 0)
    {
        var agreement = new Agreement(AgreementOwner.Supplier, supplierIdentifier, customerIdentifier, catalogIdentifier);
        agreement.SetDelivery(deliveryDays, orderDelayInHoursBeforeDeliveryDay);

        return agreement;
    }
    
    public AgreementId Id { get; }
    public AgreementStatus Status { get; private set; }
    public AgreementOwner Owner { get; private set; }
    public DateTimeOffset CreatedOn { get; private set; }
    public DateTimeOffset UpdatedOn { get; private set; }
    public int OrderDelayInHoursBeforeDeliveryDay { get; private set; }
    public SupplierId SupplierId { get; private set; }
    public CustomerId CustomerId { get; private set; }
    public CatalogId CatalogId { get; private set; }
    public string? FailureReason { get; private set; }
    public IEnumerable<DeliveryDay> DeliveryDays { get; private set; } = new List<DeliveryDay>();

    public Result SetDelivery(IEnumerable<DeliveryDay> deliveryDays, int? orderDelayInHoursBeforeDeliveryDay)
    {
        if (Status != AgreementStatus.Pending && Status != AgreementStatus.Active)
            return Result.Failure(ErrorKind.BadRequest, "agreement.delivery.requires.pending.or.active.status");
        
        var days = deliveryDays?.Distinct().ToList()  ?? new List<DeliveryDay>();
        if (days.Count == 0)
            return Result.Failure(ErrorKind.BadRequest, "agreement.delivery.days.required");
        
        DeliveryDays = new List<DeliveryDay>(days);
        OrderDelayInHoursBeforeDeliveryDay = orderDelayInHoursBeforeDeliveryDay ?? 0;
        UpdatedOn = DateTimeOffset.UtcNow;

        return Result.Success();
    }

    public Result Accept(IEnumerable<DeliveryDay>? deliveryDays = null, int? orderDelayInHoursBeforeDeliveryDay = null)
    {
        if (Status != AgreementStatus.Pending)
            return Result.Failure(ErrorKind.BadRequest, "agreement.accept.requires.pending");

        Status = AgreementStatus.Active;
        if (Owner == AgreementOwner.Supplier) 
            return Result.Success();
        
        if(deliveryDays == null || orderDelayInHoursBeforeDeliveryDay == null)
            return Result.Failure(ErrorKind.BadRequest, "agreement.accept.requires.days.or.delay");
                
        UpdatedOn = DateTimeOffset.UtcNow;
        return SetDelivery(deliveryDays, orderDelayInHoursBeforeDeliveryDay);
    }

    public Result Revoke(string revokeReason)
    {
        if(string.IsNullOrWhiteSpace(revokeReason))
            return Result.Failure(ErrorKind.BadRequest, "agreement.revoke.requires.reason");

        if (Status != AgreementStatus.Active)
            return Result.Failure(ErrorKind.BadRequest, "agreement.revoke.requires.active");
        
        Status = AgreementStatus.Revoked;
        FailureReason = revokeReason;
        UpdatedOn = DateTimeOffset.UtcNow;
        return Result.Success();
    }

    public Result Refuse(string? refusalReason = null)
    {
        if (Status != AgreementStatus.Pending)
            return Result.Failure(ErrorKind.BadRequest, "agreement.refuse.requires.pending");

        Status = AgreementStatus.Refused;
        FailureReason = refusalReason;
        UpdatedOn = DateTimeOffset.UtcNow;
        return Result.Success();
    }
}