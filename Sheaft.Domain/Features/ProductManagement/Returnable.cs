namespace Sheaft.Domain.ProductManagement;

public class Returnable : Entity
{
    private Returnable()
    {
    }
    
    public Returnable(ReturnableName name, ReturnableReference reference, UnitPrice unitPrice, VatRate vat, SupplierId supplierId)
    {
        Id = ReturnableId.New();
        Name = name;
        Reference = reference;
        UnitPrice = unitPrice;
        Vat = vat;
        SupplierId = supplierId;
        CreatedOn = DateTimeOffset.UtcNow;
        UpdatedOn = DateTimeOffset.UtcNow;
    }

    public ReturnableId Id { get; }
    public ReturnableName Name { get; private set; }
    public ReturnableReference Reference { get; private set; }
    public UnitPrice UnitPrice { get; private set; }
    public VatRate Vat { get; private set; }
    public SupplierId SupplierId { get; }
    public DateTimeOffset CreatedOn { get; private set; }
    public DateTimeOffset UpdatedOn { get; private set; }

    public Result UpdateInfo(ReturnableName name, ReturnableReference reference, UnitPrice price, VatRate vat)
    {
        Name = name;
        Reference = reference;
        UnitPrice = price;
        Vat = vat;
        UpdatedOn = DateTimeOffset.UtcNow;

        return Result.Success();
    }
}