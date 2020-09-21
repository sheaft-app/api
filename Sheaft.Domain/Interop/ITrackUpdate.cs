using System;

namespace Sheaft.Domain.Interop
{
    public interface ITrackUpdate
    {
        DateTimeOffset? UpdatedOn { get; }
    }
}