namespace Sheaft.Domain.ProductManagement;

public class Returnable : Entity
{
    private Returnable()
    {
    }
    
    public Returnable(ReturnableName name, ReturnableReference reference, UnitPrice unitPrice, VatRate vat, SupplierId supplierIdentifier)
    {
        Identifier = ReturnableId.New();
        Name = name;
        Reference = reference;
        UnitPrice = unitPrice;
        Vat = vat;
        SupplierIdentifier = supplierIdentifier;
    }

    public ReturnableId Identifier { get; }
    public ReturnableName Name { get; private set; }
    public ReturnableReference Reference { get; private set; }
    public UnitPrice UnitPrice { get; private set; }
    public VatRate Vat { get; private set; }
    public SupplierId SupplierIdentifier { get; private set; }
}