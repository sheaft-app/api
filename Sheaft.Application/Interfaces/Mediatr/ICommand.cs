using MediatR;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Mediatr
{
    public interface ICommand : IRequest<Result>, ITrackedUser
    { 
    }

    public interface ICommand<T> : IRequest<Result<T>>, ITrackedUser
    {
    }
}