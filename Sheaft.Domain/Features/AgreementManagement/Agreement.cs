using System.Collections.ObjectModel;

namespace Sheaft.Domain.AgreementManagement;

public class Agreement : AggregateRoot
{
    public Agreement(AgreementOwner owner, SupplierId supplierIdentifier, CustomerId customerIdentifier, CatalogId catalogIdentifier)
    {
        Identifier = AgreementId.New();
        Owner = owner;
        SupplierIdentifier = supplierIdentifier;
        CustomerIdentifier = customerIdentifier;
        CatalogIdentifier = catalogIdentifier;
        Status = AgreementStatus.Pending;
    }
    
    public AgreementId Identifier { get; }
    public AgreementStatus Status { get; private set; }
    public AgreementOwner Owner { get; private set; }
    public int OrderDelayInHoursBeforeDeliveryDay { get; private set; }
    public SupplierId SupplierIdentifier { get; private set; }
    public CustomerId CustomerIdentifier { get; private set; }
    public CatalogId CatalogIdentifier { get; private set; }
    public ReadOnlyCollection<DeliveryDay> DeliveryDays { get; private set; } = new ReadOnlyCollection<DeliveryDay>(new List<DeliveryDay>());

    public void SetDelivery(List<DeliveryDay> deliveryDays, int orderDelayInHoursBeforeDeliveryDay)
    {
        DeliveryDays = new ReadOnlyCollection<DeliveryDay>(deliveryDays?.Distinct().ToList() ?? new List<DeliveryDay>());
        OrderDelayInHoursBeforeDeliveryDay = orderDelayInHoursBeforeDeliveryDay;
    }
}

public enum AgreementStatus
{
    Pending,
    Active,
    Revoked
}