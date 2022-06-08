namespace Sheaft.Domain;

public record ReturnableName
{
    public const int MAXLENGTH = 80;
    private ReturnableName(){}

    public ReturnableName(string value)
    {
        // if (string.IsNullOrWhiteSpace(value))
        //     throw new InvalidOperationException("returnable.name.required");
        
        Value = value;
    }

    public string Value { get; }
}