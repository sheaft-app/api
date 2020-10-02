using MediatR;
using Sheaft.Application.Interop;
using Sheaft.Core;

namespace Sheaft.Application.Events
{
    public abstract class Event : IEvent
    {
        protected Event(RequestUser requestUser)
        {
            RequestUser = requestUser;
        }

        public RequestUser RequestUser { get; protected set; }
    }
}
