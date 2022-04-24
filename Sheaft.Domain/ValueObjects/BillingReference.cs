using System.Text.RegularExpressions;

namespace Sheaft.Domain;

public record BillingReference
{
    public const int MAXLENGTH = 20;
    protected BillingReference(){}

    public string Value { get; protected set; }
}

public record InvoiceReference : BillingReference
{
    public const int MAXLENGTH = 20;
    private readonly string ReferenceFormat = "FCT{0}-#####";
    public const string VALIDATION_REGEX = "^FCT(?<Year>[0-9]{4})-(?<InvoiceNumber>[0-9]{5})$";
    
    private InvoiceReference(){}
    
    public InvoiceReference(int value, DateTimeOffset? currentDateTime = null)
    {
        Value = value.ToString(string.Format(ReferenceFormat, (currentDateTime ?? DateTimeOffset.UtcNow).Year));
    }
    
    public InvoiceReference(string value)
    {
        if (!Regex.IsMatch(value, VALIDATION_REGEX))
            throw new InvalidOperationException("invoice.reference.invalid");
        
        Value = value;
    }

    public static Result<InvoiceReference> Next(InvoiceReference currentReference)
    {
        var regexMatch = Regex.Match(currentReference.Value, VALIDATION_REGEX);
        if (!regexMatch.Success)
            return Result.Failure<InvoiceReference>(ErrorKind.BadRequest, "invoice.reference.next.invalid.reference");

        var invoiceNumber = int.Parse(regexMatch.Groups["InvoiceNumber"].Value);
        return Result.Success(new InvoiceReference(++invoiceNumber));
    }
}