using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Mediator;
using Sheaft.Domain.Common;

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
        public async Task<Result> Execute(string jobName, ICommand<Result> data, CancellationToken token)
        {
            return await _mediatr.Send(data, token);
        }

        [DisplayName("{0}")]
        public async Task<Result<T>> Execute<T>(string jobName, ICommand<Result<T>> data, CancellationToken token)
        {
            return await _mediatr.Send(data, token);
        }

        [DisplayName("{0}")]
        public async Task Execute(string jobName, IIntegrationEvent data, CancellationToken token)
        {
            await _mediatr.Publish(GetNotificationCorrespondingToDomainEvent(data), token);
        }

        private INotification GetNotificationCorrespondingToDomainEvent(IIntegrationEvent domainEvent)
        {
            return (INotification)Activator.CreateInstance(
                typeof(WrappedEventNotification<>).MakeGenericType(domainEvent.GetType()), domainEvent);
        }
    }
}