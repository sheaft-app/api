using Sheaft.Domain;
using Sheaft.Domain.Common;

namespace Sheaft.Application.Mediator
{
    public interface ICommand<out T> : MediatR.IRequest<T>
        where T: IResult
    {
        RequestUser RequestUser { get; }
    }
}