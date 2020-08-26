using MediatR;
using Sheaft.Core;
using Sheaft.Interop;

namespace Sheaft.Application.Commands
{
    public interface ICommand<T>: IRequest<CommandResult<T>>, ITrackedUser
    {
    }

    public abstract class Command<T> : ICommand<T>
    {
        protected Command(RequestUser requestUser)
        {
            RequestUser = requestUser;
        }

        public RequestUser RequestUser { get; protected set; }
    }
}
