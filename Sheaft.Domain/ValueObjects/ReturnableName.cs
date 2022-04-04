namespace Sheaft.Domain;

public record ReturnableName
{
    private ReturnableName(){}

    public ReturnableName(string value)
    {
        Value = value;
    }

    public string Value { get; }
}