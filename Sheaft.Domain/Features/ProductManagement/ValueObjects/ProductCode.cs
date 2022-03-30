namespace Sheaft.Domain.ProductManagement;

public record ProductCode
{
    private ProductCode(){}
    
    public ProductCode(string value)
    {
        Value = value;
    }

    public string Value { get; }
}