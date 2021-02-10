using MediatR;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Models
{
    public interface ICommand : IRequest<Result>, ITrackedUser
    { 
    }

    public interface ICommand<T> : IRequest<Result<T>>, ITrackedUser
    {
    }
}