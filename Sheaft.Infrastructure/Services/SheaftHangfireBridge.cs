using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
using Sheaft.Application.Common.Models;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Infrastructure.Services
{
    public class SheaftHangfireBridge : ISheaftHangfireBridge
    {
        private readonly IMediator _mediatr;

        public SheaftHangfireBridge(IMediator mediator)
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