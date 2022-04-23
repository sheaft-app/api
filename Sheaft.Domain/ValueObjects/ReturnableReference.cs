namespace Sheaft.Domain;

public record ReturnableReference
{
    public const int MAXLENGTH = 20;
    private ReturnableReference(){}

    public ReturnableReference(string value)
    {
        Value = value;
    }

    public string Value { get; }
}