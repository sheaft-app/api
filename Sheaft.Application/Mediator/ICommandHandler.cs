using Sheaft.Domain.Common;

namespace Sheaft.Application.Mediator
{
    public abstract class BaseCommand : MediatR.IRequest<Result>
    {
        protected BaseCommand(RequestUser requestUser)
        {
            RequestUser = requestUser;
        }

        public RequestUser RequestUser { get; private set; }

        public virtual void SetRequestUser(RequestUser user)
        {
            RequestUser = user;
        }
    }
    
    public abstract class BaseCommand<T> : MediatR.IRequest<Result<T>>
    {
        protected BaseCommand(RequestUser requestUser)
        {
            RequestUser = requestUser;
        }

        public RequestUser RequestUser { get; }
    }

    public interface ICommandHandler<in T> : MediatR.IRequestHandler<T, Result>
        where T : BaseCommand
    {
    }

    public interface ICommandHandler<in T, TU> : MediatR.IRequestHandler<T, Result<TU>>
        where T : BaseCommand<TU>
    {
    }
}