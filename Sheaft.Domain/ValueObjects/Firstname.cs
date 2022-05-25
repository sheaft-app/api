namespace Sheaft.Domain;

public record Firstname
{
    public const int MAXLENGTH = 30;
    private Firstname(){}

    public Firstname(string value)
    {
        Value = value;
    }

    public string Value { get; }
}