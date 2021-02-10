namespace Sheaft.Domain.Enum
{
    public enum DeclarationStatus
    {
        Incomplete = 0,
        Created = 1,
        ValidationAsked = 2,
        Validated = 3,
        Refused = 4,
        Locked = 100,
        UnLocked
    }
}