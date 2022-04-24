using System.Text.RegularExpressions;

namespace Sheaft.Domain;

public record DeliveryNoteReference
{
    public const int MAXLENGTH = 20;
    private readonly string ReferenceFormat = "BL{0}-#####";
    public const string VALIDATION_REGEX = "^BL(?<Year>[0-9]{4})-(?<DeliveryNumber>[0-9]{5})$";

    private DeliveryNoteReference()
    {
    }

    internal DeliveryNoteReference(DeliveryReference deliveryReference)
    {
        var matchResult = Regex.Match(deliveryReference.Value, DeliveryReference.VALIDATION_REGEX);
        if (!matchResult.Success)
            throw new InvalidOperationException("deliverynote.reference.invalid.delivery");

        var value = int.Parse(matchResult.Groups["DeliveryNumber"].Value);
        var deliveryReferenceYear = int.Parse(matchResult.Groups["Year"].Value);
        
        Value = value.ToString(string.Format(ReferenceFormat, deliveryReferenceYear));
    }

    internal DeliveryNoteReference(string value)
    {
        if (!Regex.IsMatch(value, VALIDATION_REGEX))
            throw new InvalidOperationException("deliverynote.reference.invalid");

        Value = value;
    }

    public string Value { get; }
}