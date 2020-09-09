using System;

namespace Sheaft.Interop
{
    public interface ITrackCreation
    {
        DateTimeOffset CreatedOn { get; }
    }

    public interface ITrackUpdate
    {
        DateTimeOffset? UpdatedOn { get; }
    }

    public interface ITrackRemove
    {
        DateTimeOffset? RemovedOn { get; }
        void Remove();
        void Restore();
    }

    public interface IIdEntity
    {
        Guid Id { get; }
    }

    public interface IEntity : IIdEntity, ITrackCreation, ITrackUpdate, ITrackRemove
    {
    }
}