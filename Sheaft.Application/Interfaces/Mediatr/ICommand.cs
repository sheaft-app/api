using System;
using MediatR;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Mediatr
{
    public interface IJobCommand : ICommand
    {
        public Guid JobId { get; }
    }
    
    public interface ICommand : IRequest<Result>, ITrackedUser
    {
        void SetRequestUser(RequestUser user);
    }

    public interface ICommand<T> : IRequest<Result<T>>, ITrackedUser
    {
        void SetRequestUser(RequestUser user);
    }
}