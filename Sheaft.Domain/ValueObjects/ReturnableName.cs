namespace Sheaft.Domain;

public record ReturnableName
{
    public const int MAXLENGTH = 80;
    private ReturnableName(){}

    public ReturnableName(string value)
    {
        Value = value;
    }

    public string Value { get; }
}