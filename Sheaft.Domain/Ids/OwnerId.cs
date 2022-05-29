namespace Sheaft.Domain;

public record OwnerId
{
    public OwnerId(string value)
    {
        Value = value;
    }
    
    public OwnerId(SupplierId value)
    {
        Value = value.Value;
    }
    
    public OwnerId(CustomerId value)
    {
        Value = value.Value;
    }

    public string Value { get; }

    public static OwnerId New()
    {
        return new OwnerId(Nanoid.Nanoid.Generate(Constants.IDS_ALPHABET, Constants.IDS_LENGTH));
    }
}