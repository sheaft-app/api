using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Common;

namespace Sheaft.Services
{
    public class SheaftDispatcher : ISheaftDispatcher
    {
        private readonly IMediator _mediatr;

        public SheaftDispatcher(IMediator mediator)
        {
            _mediatr = mediator;
        }

        [DisplayName("{0}")]
        public async Task<Result<T>> Execute<T>(string jobname, IRequest<Result<T>> data, CancellationToken token)
        {
            var result = await _mediatr.Send(data, token);
            if (result.Succeeded)
                return result;

            if (result.Exception != null)
                throw result.Exception;

            throw SheaftException.Unexpected(MessageKind.Unexpected);
        }

        [DisplayName("{0}")]
        public async Task<Result> Execute(string jobname, IRequest<Result> data, CancellationToken token)
        {
            var result = await _mediatr.Send(data, token);
            if (result.Succeeded)
                return result;

            if (result.Exception != null)
                throw result.Exception;

            throw SheaftException.Unexpected(MessageKind.Unexpected);
        }

        [DisplayName("{0}")]
        public async Task Execute(string jobname, DomainEvent data, CancellationToken token)
        {
            await _mediatr.Publish(GetNotificationCorrespondingToDomainEvent(data), token);
        }

        private INotification GetNotificationCorrespondingToDomainEvent(DomainEvent domainEvent)
        {
            return (INotification)Activator.CreateInstance(
                typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType()), domainEvent);
        }
    }
}