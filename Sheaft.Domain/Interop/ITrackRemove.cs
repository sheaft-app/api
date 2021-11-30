using System;

namespace Sheaft.Domain.Interop
{
    public interface ITrackRemove
    {
        bool Removed { get; }

        void Restore();
    }
}