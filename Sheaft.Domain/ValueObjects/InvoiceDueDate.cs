namespace Sheaft.Domain;

public record InvoiceDueDate
{
    private InvoiceDueDate(){}

    public InvoiceDueDate(DateTimeOffset value)
        :this(value, DateTimeOffset.UtcNow)
    {
    }

    public InvoiceDueDate(DateTimeOffset value, DateTimeOffset currentDateTime)
    {
        if (value.Date < currentDateTime.Date)
            throw new InvalidOperationException("Invoice due date must be in future");
        
        Value = value.ToUniversalTime();
    }

    public DateTimeOffset Value { get; }
}