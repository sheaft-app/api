namespace Sheaft.Domain.AccountManagement;

public record Username
{
    private Username(){}

    public Username(string value)
    {
        Value = value;
    }

    public string Value { get; }
}