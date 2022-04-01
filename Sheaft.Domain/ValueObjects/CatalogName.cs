namespace Sheaft.Domain;

public record CatalogName
{
    private CatalogName(){}

    public CatalogName(string value)
    {
        Value = value;
    }

    public string Value { get; }
}