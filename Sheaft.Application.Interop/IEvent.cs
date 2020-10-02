using MediatR;
using Sheaft.Core;

namespace Sheaft.Application.Interop
{
    public interface IEvent : INotification, ITrackedUser
    {
    }
}