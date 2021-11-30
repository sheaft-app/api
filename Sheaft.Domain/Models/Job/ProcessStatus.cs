namespace Sheaft.Domain.Enum
{
    public enum ProcessStatus
    {
        Rollbacked = -2,
        Failed,
        Cancelled,
        Pending,
        Processing,
        Paused,
        Done,
        Expired
    }
}