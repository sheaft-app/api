namespace Sheaft.Domain.Enums
{
    public enum TransactionStatus
    {
        NotSpecified = 0,
        Created = 1,
        Succeeded = 2,
        Failed = 4,
        Waiting = 100,
        Expired
    }
}