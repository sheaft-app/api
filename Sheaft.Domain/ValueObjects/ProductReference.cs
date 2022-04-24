using System.Text.RegularExpressions;

namespace Sheaft.Domain;

public record ProductReference
{
    public const int MAXLENGTH = 20;
    public const string VALIDATION_REGEX = "[0-9]{13}";
    private ProductReference(){}
    
    public ProductReference(int value)
    {
        Value = GenerateEanIdentifier(value);
    }
    
    public ProductReference(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<ProductReference> Next(ProductReference currentReference)
    {
        var regexMatch = Regex.Match(currentReference.Value, VALIDATION_REGEX);
        if (!regexMatch.Success)
            return Result.Failure<ProductReference>(ErrorKind.BadRequest, "product.reference.next.invalid.reference");

        var orderNumber = int.Parse(regexMatch.Value);
        return Result.Success(new ProductReference(++orderNumber));
    }

    private string GenerateEanIdentifier(int value)
    {
        if (value.ToString().Length >= MAXLENGTH)
            throw new ArgumentException("Invalid EAN length", nameof(value));

        var str = value.ToString("000000000000");
        var checksum = CalculateChecksum(str);

        return value.ToString("000000000000") + checksum;
    }

    private int CalculateChecksum(string codeToValidate)
    {
        if (codeToValidate == null || codeToValidate.Length != 12)
            throw new ArgumentException("Code length should be 12, i.e. excluding the checksum digit",
                nameof(codeToValidate));

        int sum = 0;
        for (int i = 0; i < 12; i++)
        {
            int v;
            if (!int.TryParse(codeToValidate[i].ToString(), out v))
                throw new ArgumentException("Invalid character encountered in specified code.", nameof(codeToValidate));
            sum += (i % 2 == 0 ? v : v * 3);
        }

        int check = 10 - (sum % 10);
        return check % 10;
    }
}