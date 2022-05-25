namespace Sheaft.Domain;

public record Lastname
{
    public const int MAXLENGTH = 50;
    private Lastname(){}

    public Lastname(string value)
    {
        Value = value;
    }

    public string Value { get; }
}