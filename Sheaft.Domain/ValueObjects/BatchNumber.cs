namespace Sheaft.Domain;

public record BatchNumber
{
    private BatchNumber(){}

    public BatchNumber(string value)
    {
        Value = value;
    }

    public string Value { get; }
}