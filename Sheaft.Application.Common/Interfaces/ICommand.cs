using MediatR;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Interfaces
{
    public interface ICommand : IRequest<Result>, ITrackedUser
    { 
    }

    public interface ICommand<T> : IRequest<Result<T>>, ITrackedUser
    {
    }
}