using System.Text.RegularExpressions;

namespace Sheaft.Domain;

public record DeliveryReference
{
    public const int MAXLENGTH = 20;
    private readonly string ReferenceFormat = "LIV{0}-#####";
    public const string VALIDATION_REGEX = "^LIV(?<Year>[0-9]{4})-(?<DeliveryNumber>[0-9]{5})$";
    private DeliveryReference(){}
    
    public DeliveryReference(int value, DateTimeOffset? currentDateTime = null)
    {
        Value = value.ToString(string.Format(ReferenceFormat, (currentDateTime ?? DateTimeOffset.UtcNow).Year));
    }
    
    public DeliveryReference(string value)
    {
        if (!Regex.IsMatch(value, VALIDATION_REGEX))
            throw new InvalidOperationException("delivery.reference.invalid");
        
        Value = value;
    }

    public string Value { get; }

    public static Result<DeliveryReference> Next(DeliveryReference currentReference)
    {
        var regexMatch = Regex.Match(currentReference.Value, VALIDATION_REGEX);
        if (!regexMatch.Success)
            return Result.Failure<DeliveryReference>(ErrorKind.BadRequest, "delivery.reference.next.invalid.reference");

        var deliveryNumber = int.Parse(regexMatch.Groups["DeliveryNumber"].Value);
        return Result.Success(new DeliveryReference(++deliveryNumber));
    }

    public DeliveryNoteReference GetDeliveryNoteReference()
    {
        return new DeliveryNoteReference(this);
    }

    public DeliveryReceiptReference GetDeliveryReceiptReference()
    {
        return new DeliveryReceiptReference(this);
    }
}