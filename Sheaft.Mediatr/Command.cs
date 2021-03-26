using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Domain;

namespace Sheaft.Mediatr
{
    public abstract class Command : ICommand
    {
        protected Command(RequestUser requestUser)
        {
            RequestUser = requestUser;
        }

        public RequestUser RequestUser { get; private set; }
    }
    
    public abstract class Command<T> : ICommand<T>
    {
        protected Command(RequestUser requestUser)
        {
            RequestUser = requestUser;
        }

        public RequestUser RequestUser { get; private set; }
    }
}
