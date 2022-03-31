namespace Sheaft.Domain;

public record AccountId(string Value)
{
    public static AccountId New()
    {
        return new AccountId(Guid.NewGuid().ToString("N"));
    }
}