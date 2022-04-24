using System.Text.RegularExpressions;

namespace Sheaft.Domain;

public record OrderReference
{
    public const int MAXLENGTH = 20;
    private readonly string ReferenceFormat = "CD{0}#####";
    public const string VALIDATION_REGEX = "CD(?<Year>[0-9]{4})(?<OrderNumber>[0-9]{5})";
    private OrderReference(){}
    
    public OrderReference(int value)
    {
        Value = value.ToString(string.Format(ReferenceFormat, DateTimeOffset.UtcNow.Year));
    }
    
    public OrderReference(string value)
    {
        if (!Regex.IsMatch(value, VALIDATION_REGEX))
            throw new InvalidOperationException("order.reference.invalid");
        
        Value = value;
    }

    public string Value { get; }

    public static Result<OrderReference> Next(OrderReference currentReference)
    {
        var regexMatch = Regex.Match(currentReference.Value, VALIDATION_REGEX);
        if (!regexMatch.Success)
            return Result.Failure<OrderReference>(ErrorKind.BadRequest, "order.reference.next.invalid.reference");

        var orderNumber = int.Parse(regexMatch.Groups["OrderNumber"].Value);
        return Result.Success(new OrderReference(++orderNumber));
    }
}