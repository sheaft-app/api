using MediatR;
using Sheaft.Core;

namespace Sheaft.Application.Interop
{
    public interface ICommand<T> : IRequest<Result<T>>, ITrackedUser
    {
        void RemoveUserInfos();
    }
}