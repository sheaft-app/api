using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Domain;

namespace Sheaft.Mediatr
{
    public abstract class Command : ICommand
    {
        protected Command(){}
        
        protected Command(RequestUser requestUser)
        {
            RequestUser = requestUser;
        }

        public RequestUser RequestUser { get; private set; }

        public virtual void SetRequestUser(RequestUser user)
        {
            RequestUser = user;
        }
    }
    
    public abstract class Command<T> : ICommand<T>
    {
        protected Command(){}
        
        protected Command(RequestUser requestUser)
        {
            RequestUser = requestUser;
        }

        public RequestUser RequestUser { get; private set; }

        public virtual void SetRequestUser(RequestUser user)
        {
            RequestUser = user;
        }
    }
}
