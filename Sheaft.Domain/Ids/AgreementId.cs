namespace Sheaft.Domain;

public record AgreementId(string Value)
{
    public static AgreementId New()
    {
        return new AgreementId(Nanoid.Nanoid.Generate(Constants.IDS_ALPHABET, Constants.IDS_LENGTH));
    }
}