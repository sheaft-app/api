namespace Sheaft.Domain.Enum
{
    public enum DocumentStatus
    {
        NotSpecified = 0,
        Created = 1,
        ValidationAsked = 2,
        Validated = 4,
        Refused = 8,
        OutOfDate = 16,
        Locked = 100,
        UnLocked = 101,
    }
}