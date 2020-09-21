using System;

namespace Sheaft.Domain.Interop
{
    public interface ITrackRemove
    {
        DateTimeOffset? RemovedOn { get; }
    }
}