﻿using System.Collections.ObjectModel;

namespace Sheaft.Domain.AgreementManagement;

public class Agreement : AggregateRoot
{
    private Agreement(){}
    
    private Agreement(AgreementOwner owner, SupplierId supplierIdentifier, CustomerId customerIdentifier, CatalogId catalogIdentifier)
    {
        Identifier = AgreementId.New();
        Owner = owner;
        SupplierIdentifier = supplierIdentifier;
        CustomerIdentifier = customerIdentifier;
        CatalogIdentifier = catalogIdentifier;
        Status = AgreementStatus.Pending;
    }

    public static Agreement CreateCustomerAgreement(SupplierId supplierIdentifier, CustomerId customerIdentifier,
        CatalogId catalogIdentifier)
    {
        return new Agreement(AgreementOwner.Customer, supplierIdentifier, customerIdentifier, catalogIdentifier);
    }

    public static Agreement CreateSupplierAgreement(SupplierId supplierIdentifier, CustomerId customerIdentifier,
        CatalogId catalogIdentifier, List<DeliveryDay> deliveryDays, int? orderDelayInHoursBeforeDeliveryDay = 0)
    {
        var agreement = new Agreement(AgreementOwner.Supplier, supplierIdentifier, customerIdentifier, catalogIdentifier);
        agreement.SetDelivery(deliveryDays, orderDelayInHoursBeforeDeliveryDay);

        return agreement;
    }
    
    public AgreementId Identifier { get; }
    public AgreementStatus Status { get; private set; }
    public AgreementOwner Owner { get; private set; }
    public int OrderDelayInHoursBeforeDeliveryDay { get; private set; }
    public SupplierId SupplierIdentifier { get; private set; }
    public CustomerId CustomerIdentifier { get; private set; }
    public CatalogId CatalogIdentifier { get; private set; }
    public string? RevokedReason { get; private set; }
    public ReadOnlyCollection<DeliveryDay> DeliveryDays { get; private set; } = new ReadOnlyCollection<DeliveryDay>(new List<DeliveryDay>());

    public Result SetDelivery(List<DeliveryDay> deliveryDays, int? orderDelayInHoursBeforeDeliveryDay)
    {
        if (Status != AgreementStatus.Pending && Status != AgreementStatus.Active)
            return Result.Failure(ErrorKind.BadRequest, "agreement.delivery.requires.pending.or.active.status");
        
        var days = deliveryDays?.Distinct().ToList()  ?? new List<DeliveryDay>();
        if (days.Count == 0)
            return Result.Failure(ErrorKind.BadRequest, "agreement.delivery.days.required");
        
        DeliveryDays = new ReadOnlyCollection<DeliveryDay>(days);
        OrderDelayInHoursBeforeDeliveryDay = orderDelayInHoursBeforeDeliveryDay ?? 0;

        return Result.Success();
    }

    public Result Accept(List<DeliveryDay>? deliveryDays = null, int? orderDelayInHoursBeforeDeliveryDay = null)
    {
        if (Status != AgreementStatus.Pending)
            return Result.Failure(ErrorKind.BadRequest, "agreement.accept.requires.pending");

        Status = AgreementStatus.Active;
        if (Owner == AgreementOwner.Supplier) 
            return Result.Success();
        
        if(deliveryDays == null || orderDelayInHoursBeforeDeliveryDay == null)
            return Result.Failure(ErrorKind.BadRequest, "agreement.accept.requires.days.or.delay");
                
        return SetDelivery(deliveryDays, orderDelayInHoursBeforeDeliveryDay);
    }

    public Result Revoke(string reason)
    {
        if (Status != AgreementStatus.Active)
            return Result.Failure(ErrorKind.BadRequest, "agreement.revoke.requires.active");
        
        Status = AgreementStatus.Revoked;
        RevokedReason = reason;
        return Result.Success();
    }

    public Result Refuse()
    {
        if (Status != AgreementStatus.Pending)
            return Result.Failure(ErrorKind.BadRequest, "agreement.refuse.requires.pending");

        Status = AgreementStatus.Refused;
        return Result.Success();
    }
}