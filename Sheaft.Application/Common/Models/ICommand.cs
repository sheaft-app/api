using MediatR;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Models
{
    public interface ICommand : IRequest<Result>, ITrackedUser
    { 
        void RemoveUserInfos();
    }

    public interface ICommand<T> : IRequest<Result<T>>, ITrackedUser
    {
        void RemoveUserInfos();
    }
}