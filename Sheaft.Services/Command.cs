using Sheaft.Application.Interfaces;
using Sheaft.Domain;

namespace Sheaft.Services
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
