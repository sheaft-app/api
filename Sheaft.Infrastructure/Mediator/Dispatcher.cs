using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application;
using Sheaft.Application.Mediator;
using Sheaft.Domain.Common;
using Sheaft.Domain.Events;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Infrastructure.Mediator
{
    internal class Dispatcher : IDispatcher
    {
        private readonly MediatR.IMediator _mediatr;

        public Dispatcher(MediatR.IMediator mediator)
        {
            _mediatr = mediator;
        }

        [DisplayName("{0}")]
        public async Task<Result<T>> Execute<T>(string jobName, MediatR.IRequest<Result<T>> data, CancellationToken token)
        {
            var result = await _mediatr.Send(data, token);
            if (result.Succeeded)
                return result;

            throw new SheaftException(result);
        }

        [DisplayName("{0}")]
        public async Task<Result> Execute(string jobName, MediatR.IRequest<Result> data, CancellationToken token)
        {
            var result = await _mediatr.Send(data, token);
            if (result.Succeeded)
                return result;
                
            throw new SheaftException(result);
        }

        [DisplayName("{0}")]
        public async Task Execute(string jobName, DomainEvent data, CancellationToken token)
        {
            await _mediatr.Publish(GetNotificationCorrespondingToDomainEvent(data), token);
        }

        private MediatR.INotification GetNotificationCorrespondingToDomainEvent(DomainEvent domainEvent)
        {
            return (MediatR.INotification)Activator.CreateInstance(
                typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType()), domainEvent);
        }
    }
}