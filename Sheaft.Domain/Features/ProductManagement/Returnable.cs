namespace Sheaft.Domain.ProductManagement;

public class Returnable : Entity
{
    private Returnable()
    {
    }
    
    public Returnable(ReturnableName name, ReturnableReference reference, Price price, VatRate vat)
    {
        Identifier = ReturnableId.New();
        Name = name;
        Reference = reference;
        Price = price;
        Vat = vat;
    }

    public ReturnableId Identifier { get; }
    public ReturnableName Name { get; private set; }
    public ReturnableReference Reference { get; private set; }
    public Price Price { get; private set; }
    public VatRate Vat { get; private set; }
}