namespace Sheaft.Domain;

public record BillingInformation
{
    protected BillingInformation(){}
    
    protected BillingInformation(string name, EmailAddress email, Siret siret, Address address)
    {
        Name = name;
        Email = email;
        Siret = siret;
        Address = address;
    }

    public string Name { get; init; }
    public EmailAddress Email { get; init; }
    public Siret Siret { get; init; }
    public Address Address { get; init; }
}

public record SupplierBillingInformation : BillingInformation
{
    private SupplierBillingInformation(){}

    public SupplierBillingInformation(SupplierId identifier, string name, EmailAddress email, Siret siret, Address address) 
        : base(name, email, siret, address)
    {
        Identifier = identifier;
    }
    
    public SupplierId Identifier { get; }

    public static SupplierBillingInformation Copy(SupplierBillingInformation supplier)
    {
        return new SupplierBillingInformation(supplier.Identifier, supplier.Name, supplier.Email, supplier.Siret,
            Domain.Address.Copy(supplier.Address));
    }
}

public record CustomerBillingInformation : BillingInformation
{
    public CustomerId Identifier { get; }

    private CustomerBillingInformation(){}

    public CustomerBillingInformation(CustomerId identifier, string name, EmailAddress email, Siret siret, Address address) 
        : base(name, email, siret, address)
    {
        Identifier = identifier;
    }

    public static CustomerBillingInformation Copy(CustomerBillingInformation customer)
    {
        return new CustomerBillingInformation(customer.Identifier, customer.Name, customer.Email, customer.Siret,
            Domain.Address.Copy(customer.Address));
    }
}