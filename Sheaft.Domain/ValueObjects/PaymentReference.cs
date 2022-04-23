namespace Sheaft.Domain;

public record PaymentReference
{
    public const int MAXLENGTH = 20;
    private PaymentReference(){}
    
    public PaymentReference(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException("Payment reference is required");

        Value = value;
    }

    public string Value { get; }
}