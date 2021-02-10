namespace Sheaft.Domain.Enum
{
    public enum ProcessStatus
    {
        Rollbacked = -2,
        Failed,
        Cancelled,
        Waiting,
        Processing,
        Paused,
        Done,
        Expired
    }
}