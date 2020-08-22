using MediatR;
using Sheaft.Core;

namespace Sheaft.Application.Events
{
    public interface IEvent : INotification, ITrackedUser
    {
    }

    public abstract class Event : IEvent
    {
        protected Event(RequestUser user)
        {
            RequestUser = user;
        }

        public RequestUser RequestUser { get; }
    }
}
