using Sheaft.Domain.Common;

namespace Sheaft.Application.Mediator
{
    public abstract class Command : MediatR.IRequest
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
    
    public abstract class Command<T> : MediatR.IRequest<T>
    {
        protected Command(){}
        
        protected Command(RequestUser requestUser)
        {
            RequestUser = requestUser;
        }

        public RequestUser RequestUser { get; }
    }
}
