using System.Text.RegularExpressions;

namespace Sheaft.Domain;

public record CreditNoteReference : BillingReference
{
    public const int MAXLENGTH = 20;
    private readonly string ReferenceFormat = "AVR{0}-#####";
    public const string VALIDATION_REGEX = "^AVR(?<Year>[0-9]{4})-(?<InvoiceNumber>[0-9]{5})$";
    
    private CreditNoteReference(){}
    
    public CreditNoteReference(int value, DateTimeOffset? currentDateTime = null)
    {
        Value = value.ToString(string.Format(ReferenceFormat, (currentDateTime ?? DateTimeOffset.UtcNow).Year));
    }
    
    public CreditNoteReference(string value)
    {
        if (!Regex.IsMatch(value, VALIDATION_REGEX))
            throw new InvalidOperationException("creditnote.reference.invalid");
        
        Value = value;
    }

    public static Result<CreditNoteReference> Next(CreditNoteReference currentReference)
    {
        var regexMatch = Regex.Match(currentReference.Value, VALIDATION_REGEX);
        if (!regexMatch.Success)
            return Result.Failure<CreditNoteReference>(ErrorKind.BadRequest, "creditnote.reference.next.invalid.reference");

        var creditnoteNumber = int.Parse(regexMatch.Groups["InvoiceNumber"].Value);
        return Result.Success(new CreditNoteReference(++creditnoteNumber));
    }
}