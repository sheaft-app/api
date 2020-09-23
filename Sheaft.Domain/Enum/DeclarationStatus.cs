namespace Sheaft.Domain.Enums
{
    public enum DeclarationStatus
    {
        Incomplete = 0,
        Created = 1,
        ValidationAsked = 2,
        Validated = 3,
        Refused = 4,
        WaitingForCreation = 100,
        WaitingForReview
    }
}