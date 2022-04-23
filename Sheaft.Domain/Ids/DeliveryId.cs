namespace Sheaft.Domain;

public record DeliveryId(string Value)
{
    public static DeliveryId New()
    {
        return new DeliveryId(Nanoid.Nanoid.Generate(Constants.IDS_ALPHABET, Constants.IDS_LENGTH));
    }
}