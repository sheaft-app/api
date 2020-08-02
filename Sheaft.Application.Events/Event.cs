using MediatR;
using Sheaft.Interop;

namespace Sheaft.Application.Events
{
    public interface IEvent : INotification, ITrackedUser
    {
    }

    public abstract class Event : IEvent
    {
        protected Event(IRequestUser user)
        {
            RequestUser = user;
        }

        public IRequestUser RequestUser { get; }
    }
}
