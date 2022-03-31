namespace Sheaft.Domain;

public record AgreementId(string Value)
{
    public static AgreementId New()
    {
        return new AgreementId(Guid.NewGuid().ToString("N"));
    }
}