using System;
using Sheaft.Domain.Common;

namespace Sheaft.Application.Mediator
{
    public interface IJobCommand : ICommand
    {
        public Guid JobId { get; }
    }
    
    public interface ICommand : MediatR.IRequest<Result>, ITrackedUser
    {
    }

    public interface ICommand<T> : MediatR.IRequest<Result<T>>, ITrackedUser
    {
    }
}