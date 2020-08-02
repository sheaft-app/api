namespace Sheaft.Interop.Enums
{
    public enum ProcessStatusKind
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