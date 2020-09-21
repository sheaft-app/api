using System;

namespace Sheaft.Domain.Interop
{
    public interface ITrackCreation
    {
        DateTimeOffset CreatedOn { get; }
    }
}