namespace Sheaft.Domain;

public record DocumentName
{
    public const int MAXLENGTH = 150;
    private DocumentName(){}

    public DocumentName(string value)
    {
        Value = value;
    }

    public string Value { get; }
}