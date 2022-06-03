using System.Text.RegularExpressions;

namespace Sheaft.Domain;

public record ReturnableReference
{
    public const int MAXLENGTH = 20;
    public const string VALIDATION_REGEX = "^[0-9]{13}$";
    private ReturnableReference(){}
    
    public ReturnableReference(long value)
    {
        if (value < 100000000000)
            value += 100000000000;
        
        Value = EanGenerator.Generate(value);
    }
    
    public ReturnableReference(string value)
    {
        Value = value.Replace(" ", "-").ToUpperInvariant();
    }

    public string Value { get; }

    public static Result<ReturnableReference> Next(ReturnableReference currentReference)
    {
        var regexMatch = Regex.Match(currentReference.Value, VALIDATION_REGEX);
        if (!regexMatch.Success)
            return Result.Failure<ReturnableReference>(ErrorKind.BadRequest, "returnable.reference.next.invalid.reference");

        var currentId = regexMatch.Value.Substring(0, regexMatch.Value.Length - 1);
        var orderNumber = long.Parse(currentId);
        return Result.Success(new ReturnableReference(++orderNumber));
    }
}