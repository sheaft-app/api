using System.Text.RegularExpressions;

namespace Sheaft.Domain;

public record DeliveryReceiptReference
{
    public const int MAXLENGTH = 20;
    private readonly string ReferenceFormat = "BR{0}-#####";
    public const string VALIDATION_REGEX = "^BR(?<Year>[0-9]{4})-(?<DeliveryNumber>[0-9]{5})$";

    private DeliveryReceiptReference()
    {
    }

    internal DeliveryReceiptReference(DeliveryReference deliveryReference)
    {
        var matchResult = Regex.Match(deliveryReference.Value, DeliveryReference.VALIDATION_REGEX);
        if (!matchResult.Success)
            throw new InvalidOperationException("deliveryreceipt.reference.invalid.delivery");

        var value = int.Parse(matchResult.Groups["DeliveryNumber"].Value);
        var deliveryReferenceYear = int.Parse(matchResult.Groups["Year"].Value);
        
        Value = value.ToString(string.Format(ReferenceFormat, deliveryReferenceYear));
    }

    internal DeliveryReceiptReference(string value)
    {
        if (!Regex.IsMatch(value, VALIDATION_REGEX))
            throw new InvalidOperationException("deliveryreceipt.reference.invalid");

        Value = value;
    }

    public string Value { get; }
}