namespace Sheaft.Domain.Interop
{
    public interface IEntity : IIdEntity, ITrackCreation, ITrackUpdate, ITrackRemove
    {
    }
}