namespace Sheaft.Domain;

public record InvoiceReference
{
    private InvoiceReference(){}
    
    public InvoiceReference(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException("Invoice reference is required");
        
        Value = value;
    }

    public string Value { get; }
}