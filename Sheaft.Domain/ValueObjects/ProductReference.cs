using System.Text.RegularExpressions;

namespace Sheaft.Domain;

public record ProductReference
{
    public const int MAXLENGTH = 20;
    public const string VALIDATION_REGEX = "^[0-9]{13}$";
    private ProductReference(){}
    
    public ProductReference(long value)
    {
        Value = EanGenerator.Generate(value);
    }
    
    public ProductReference(string value)
    {
        Value = value.Replace(" ", "-").ToUpperInvariant();
    }

    public string Value { get; }

    public static Result<ProductReference> Next(ProductReference currentReference)
    {
        var regexMatch = Regex.Match(currentReference.Value, VALIDATION_REGEX);
        if (!regexMatch.Success)
            return Result.Failure<ProductReference>(ErrorKind.BadRequest, "product.reference.next.invalid.reference");

        var currentId = regexMatch.Value.Substring(0, regexMatch.Value.Length - 1);
        var orderNumber = long.Parse(currentId);
        return Result.Success(new ProductReference(++orderNumber));
    }
}