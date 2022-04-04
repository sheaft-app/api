namespace Sheaft.Domain;

public record ReturnableReference
{
    private ReturnableReference(){}

    public ReturnableReference(string value)
    {
        Value = value;
    }

    public string Value { get; }
}